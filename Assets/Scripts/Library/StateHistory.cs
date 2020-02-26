using System.Linq;

namespace EntityComponentState
{
    public class StateHistory : IToBytes
    {
        protected SerializableListInt positions = new SerializableListInt();
        protected SerializableList<State> states = new SerializableList<State>();

        public int LatestTick => states.Max(state => state.tick);
        public State LatestState => states.FirstOrDefault(state => state.tick == LatestTick);

        public void Add(State state)
        {
            positions.Add(states.ToBytes().Count);
            states.Add(state);
        }

        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
            var newBaseIndex = index + count;
            if (newBaseIndex < positions.Count)
            {
                var position = positions[newBaseIndex];
                var basePosition = positions[index];
                var delta = position - basePosition;
                for (int i = newBaseIndex; i < positions.Count; i++)
                    positions[i] -= delta;
            }
            positions.RemoveRange(index, count);
            states.RemoveRange(index, count);
        }

        public State GetState(int tick)
        {
            return states.Where(state => state.tick == tick).FirstOrDefault();
        }

        public DeltaStateOld GetDeltaState(int startTick, int endTick)
        {
            var startState = GetState(startTick);
            var endState = GetState(endTick);
            return new DeltaStateOld(startState, endState);
        }

        public DeltaStateOld GetDeltaState(int startTick)
        {
            return GetDeltaState(startTick, LatestTick);
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            bytes.Enqueue(positions);
            bytes.Enqueue(states);
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            positions.FromBytes(bytes);
            states.FromBytes(bytes);
        }

        public static T GetStateFromBytes<T>(ByteQueue bytes, int tick) where T : State
        {
            var positions = new SerializableListInt();
            positions.FromBytes(bytes);
            bytes.RemoveRange(0, positions[tick]);
            return bytes.GetIToBytes<T>(typeof(T));
        }

        public static T GetLatestStateFromBytes<T>(ByteQueue bytes) where T : State
        {
            var positions = new SerializableListInt();
            positions.FromBytes(bytes);
            bytes.RemoveRange(0, positions[positions.Count - 1]);
            return bytes.GetIToBytes<T>(typeof(T));
        }
    }
}
