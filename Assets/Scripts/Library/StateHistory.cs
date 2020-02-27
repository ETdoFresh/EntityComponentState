using System;
using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public class StateHistory<T> : IToBytes where T : State
    {
        public static readonly byte[] STATE_DELIMITER = Encoding.UTF8.GetBytes("<EOL>");

        public int LatestTick => states.Max(state => state.tick);
        public State LatestState => states.FirstOrDefault(state => state.tick == LatestTick);

        protected SerializableList<T> states = new SerializableList<T>();

        public void Add(T state)
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
            states.Add(bytes.GetIToBytes<T>(typeof(T)));
            while (bytes.StartsWith(STATE_DELIMITER))
            {
                bytes.GetBytes(STATE_DELIMITER.Length);
                states.Add(bytes.GetIToBytes<T>(typeof(T)));
            }
        }

        public static T GetStateFromBytes(ByteQueue bytes, int tick)
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

        public static T GetLatestStateFromBytes(ByteQueue bytes)
        {
            return GetStateFromBytes(bytes, GetCountFromBytes(new ByteQueue(bytes)));
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
