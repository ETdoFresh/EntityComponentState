using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentState
{
    public class ByteQueue : List<byte>
    {
        public ByteQueue() : base() { }
        public ByteQueue(IEnumerable<byte> value) : base(value) { }
        public ByteQueue(byte value) : base() { Enqueue(value); }
        public ByteQueue(bool value) : base() { Enqueue(value); }
        public ByteQueue(short value) : base() { Enqueue(value); }
        public ByteQueue(ushort value) : base() { Enqueue(value); }
        public ByteQueue(int value) : base() { Enqueue(value); }
        public ByteQueue(uint value) : base() { Enqueue(value); }
        public ByteQueue(float value) : base() { Enqueue(value); }
        public ByteQueue(double value) : base() { Enqueue(value); }
        public ByteQueue(string value) : base() { Enqueue(value); }
        public ByteQueue(IToBytes value) : base() { Enqueue(value); }

        public byte GetByte()
        {
            var value = this[0];
            RemoveAt(0);
            return value;
        }

        public byte[] GetBytes(int count)
        {
            var bytes = new byte[count];
            for (int i = 0; i < count; i++)
                bytes[i] = this[i];
            RemoveRange(0, count);
            return bytes;
        }

        public bool GetBool() => BitConverter.ToBoolean(GetBytes(1), 0);
        public short GetShort() => BitConverter.ToInt16(GetBytes(2), 0);
        public ushort GetUShort() => BitConverter.ToUInt16(GetBytes(2), 0);
        public int GetInt() => BitConverter.ToInt32(GetBytes(4), 0);
        public uint GetUInt() => BitConverter.ToUInt32(GetBytes(4), 0);
        public float GetFloat() => BitConverter.ToSingle(GetBytes(4), 0);
        public double GetDouble() => BitConverter.ToDouble(GetBytes(8), 0);
        
        public string GetString()
        {
            var count = BitConverter.ToInt32(GetBytes(4), 0);
            var value = Encoding.UTF8.GetString(GetBytes(count));
            return value;
        }

        public T GetIToBytes<T>(Type type) where T : IToBytes
        {
            var instance = (T)Activator.CreateInstance(type);
            instance.FromBytes(this);
            return instance;
        }

        public void Enqueue(byte value) => Add(value);
        public void Enqueue(IEnumerable<byte> value) => AddRange(value);
        public void Enqueue(bool value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(short value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(ushort value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(int value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(uint value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(float value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(double value) => AddRange(BitConverter.GetBytes(value));
        public void Enqueue(string value) { AddRange(BitConverter.GetBytes(value.Length)); AddRange(Encoding.UTF8.GetBytes(value)); }
        public void Enqueue(IToBytes value) { AddRange(value.ToBytes()); }

        public string ToHexString()
        {
            var output = "";

            foreach (var @byte in this)
                output += $"{@byte:x2}";

            return output;
        }

        public bool StartsWith(byte[] bytes)
        {
            if (bytes.Length > Count)
                return false;

            if (bytes.Length == 0)
                return false;

            for (int i = 0; i < bytes.Length; i++)
                if (bytes[i] != this[i])
                    return false;

            return true;
        }
    }
}
