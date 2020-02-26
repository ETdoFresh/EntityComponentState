﻿using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public class StateHistory : IToBytes
    {
        public static readonly byte[] STATE_DELIMITER = Encoding.UTF8.GetBytes("<EOL>");

        public int LatestTick => states.Max(state => state.tick);
        public State LatestState => states.FirstOrDefault(state => state.tick == LatestTick);

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

        public static T GetStateFromBytes<T>(ByteQueue bytes, int tick) where T : State
        {
            if (tick == 0)
                return bytes.GetIToBytes<T>(typeof(T));

            var currentTick = 0;
            while (bytes.Count > 0)
            {
                if (bytes.StartsWith(STATE_DELIMITER))
                    currentTick++;

                if (currentTick == tick)
                {
                    bytes.GetBytes(STATE_DELIMITER.Length);
                    return bytes.GetIToBytes<T>(typeof(T));
                }

                bytes.GetByte();
            }
            return null;
        }

        public static T GetLatestStateFromBytes<T>(ByteQueue bytes) where T : State
        {
            return GetStateFromBytes<T>(bytes, GetCountFromBytes(new ByteQueue(bytes)));
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
