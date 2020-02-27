using System;
using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public class DeltaStateHistory : IToBytes
    {
        public static readonly byte[] STATE_DELIMITER = Encoding.UTF8.GetBytes("<EOL>");

        public int LatestTick => deltaStates.Max(d => d.endStateTick);
        public DeltaState LatestDeltaState => deltaStates.FirstOrDefault(d => d.endStateTick == LatestTick);

        protected SerializableList<DeltaState> deltaStates = new SerializableList<DeltaState>();

        public void Add(DeltaState deltaState)
        {
            deltaStates.Add(deltaState);
        }

        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
            deltaStates.RemoveRange(index, count);
        }

        public DeltaState GetDeltaState(int startStateTick)
        {
            return deltaStates.Where(d => d.startStateTick == startStateTick).FirstOrDefault();
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            for (int i = 0; i < deltaStates.Count; i++)
            {
                if (i > 0) bytes.Enqueue(STATE_DELIMITER);
                bytes.Enqueue(deltaStates[i]);
            }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            deltaStates.Clear();
            deltaStates.Add(bytes.GetIToBytes<DeltaState>(deltaStates.type));
            while (bytes.StartsWith(STATE_DELIMITER))
            {
                bytes.GetBytes(STATE_DELIMITER.Length);
                deltaStates.Add(bytes.GetIToBytes<DeltaState>(deltaStates.type));
            }
        }

        public static T GetDeltaStateFromBytes<T>(ByteQueue bytes, int startStateTick) where T : DeltaState
            => (T)GetStateFromBytes(typeof(T), bytes, startStateTick);

        public static DeltaState GetStateFromBytes(Type type, ByteQueue bytes, int startStateTick)
        {
            if (startStateTick == 0)
                return bytes.GetIToBytes<DeltaState>(type);

            var currentTick = 0;
            while (bytes.Count > 0)
            {
                if (bytes.StartsWith(STATE_DELIMITER))
                    currentTick++;

                if (currentTick == startStateTick)
                {
                    bytes.GetBytes(STATE_DELIMITER.Length);
                    return bytes.GetIToBytes<DeltaState>(type);
                }

                bytes.GetByte();
            }
            return null;
        }

        public static T GetLatestStateFromBytes<T>(ByteQueue bytes) where T : DeltaState
        {
            return GetDeltaStateFromBytes<T>(bytes, GetCountFromBytes(new ByteQueue(bytes)));
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
