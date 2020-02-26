using System;
using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public class StateHistory : IToBytes
    {
        public static readonly byte[] STATE_DELIMITER = Encoding.UTF8.GetBytes("<EOL>");

        public int LatestTick => states.Max(state => state.tick);
        public State LatestState => states.FirstOrDefault(state => state.tick == LatestTick);
        public DeltaState LatestDeltaState => GetLatestDeltaState();

        protected SerializableList<State> states = new SerializableList<State>();

        public void Add(State state)
        {
            states.Add(state);
        }

        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
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
            for (int i = 0; i < states.Count; i++)
            {
                if (i > 0) bytes.Enqueue(STATE_DELIMITER);
                bytes.Enqueue(states[i]);
            }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            states.Clear();
            states.Add(bytes.GetIToBytes<State>(states.type));
            while (bytes.StartsWith(STATE_DELIMITER))
            {
                bytes.GetBytes(STATE_DELIMITER.Length);
                states.Add(bytes.GetIToBytes<State>(states.type));
            }
        }

        private DeltaState GetLatestDeltaState()
        {
            if (states.Count <= 1)
                return null;

            var deltaState = (DeltaState)Activator.CreateInstance(states[0].deltaType);
            deltaState.Create(states[states.Count - 2], states[states.Count - 1]);
            return deltaState;
        }

        public static T GetStateFromBytes<T>(ByteQueue bytes, int tick) where T : State
            => (T)GetStateFromBytes(typeof(T), bytes, tick);

        public static State GetStateFromBytes(Type type, ByteQueue bytes, int tick)
        {
            if (tick == 0)
                return bytes.GetIToBytes<State>(type);

            var currentTick = 0;
            while (bytes.Count > 0)
            {
                if (bytes.StartsWith(STATE_DELIMITER))
                    currentTick++;

                if (currentTick == tick)
                {
                    bytes.GetBytes(STATE_DELIMITER.Length);
                    return bytes.GetIToBytes<State>(type);
                }

                bytes.GetByte();
            }
            return null;
        }

        public static T GetLatestStateFromBytes<T>(ByteQueue bytes) where T : State
        {
            return GetStateFromBytes<T>(bytes, GetCountFromBytes(new ByteQueue(bytes)));
        }

        public static T GetLatestDeltaStateFromBytes<T>(ByteQueue bytes) where T : DeltaState
        {
            var count = GetCountFromBytes(new ByteQueue(bytes));
            if (count <= 1)
                return null;

            var deltaState = Activator.CreateInstance<T>();
            var previousState = GetStateFromBytes(deltaState.stateType, new ByteQueue(bytes), count - 1);
            var state = GetStateFromBytes(deltaState.stateType, bytes, count);
            deltaState.Create(previousState, state);
            return deltaState;
        }

        public static int GetCountFromBytes(ByteQueue bytes)
        {
            if (bytes.Count == 0)
                return 0;

            var count = 1;
            while (bytes.Count > 0)
            {
                if (bytes.StartsWith(STATE_DELIMITER))
                    count++;

                bytes.GetByte();
            }
            return count;
        }
    }
}
