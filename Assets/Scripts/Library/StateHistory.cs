﻿using System;
using System.Linq;
using static EntityComponentState.Constants;

namespace EntityComponentState
{
    public class StateHistory<T> : IToBytes where T : State
    {
        protected SerializableList<T> states = new SerializableList<T>();

        public int Count => states.Count; 
        public T this[int index] => states[index];

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
                if (i > 0) bytes.Enqueue(DELIMITER);
                bytes.Enqueue(states[i]);
            }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            states.Clear();
            states.Add(bytes.GetIToBytes<T>(typeof(T)));
            while (bytes.StartsWith(DELIMITER))
            {
                bytes.GetBytes(DELIMITER.Length);
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

                if (bytes.StartsWith(DELIMITER))
                    currentTick++;

                if (currentTick == tick)
                {
                    bytes.GetBytes(DELIMITER.Length);
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
                if (bytes.StartsWith(DELIMITER))
                    count++;

                bytes.GetByte();
            }
            return count;
        }
    }
}
