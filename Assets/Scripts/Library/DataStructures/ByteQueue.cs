using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public class ByteQueue : List<byte>
    {
        public ByteQueue() : base() { }
        public ByteQueue(IEnumerable<byte> bytes) : base(bytes) { }
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
            var size = 1;
            var value = this.Take(size).First();
            RemoveRange(0, size);
            return value;
        }

        public bool GetBool()
        {
            var size = 1;
            var value = BitConverter.ToBoolean(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public short GetShort()
        {
            var size = 2;
            var value = BitConverter.ToInt16(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public ushort GetUShort()
        {
            var size = 2;
            var value = BitConverter.ToUInt16(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public int GetInt()
        {
            var size = 4;
            var value = BitConverter.ToInt32(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public uint GetUInt()
        {
            var size = 4;
            var value = BitConverter.ToUInt32(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public float GetFloat()
        {
            var size = 4;
            var value = BitConverter.ToSingle(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public double GetDouble()
        {
            var size = 8;
            var value = BitConverter.ToDouble(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }

        public string GetString()
        {
            var count = BitConverter.ToInt32(this.Take(4).ToArray(), 0);
            var value = Encoding.UTF8.GetString(this.Skip(4).Take(count).ToArray());
            RemoveRange(0, 4 + count);
            return value;
        }

        public T GetIToBytes<T>(Type type) where T : IToBytes
        {
            var instance = (T)Activator.CreateInstance(type);
            instance.FromBytes(this);
            return instance;
        }

        public float GetCompressedSingle(float minRange, float maxRange, int bytes)
        {
            if (bytes >= 4) return GetFloat();
            var range = maxRange - minRange;
            if (bytes == 1)
            {
                var unitIncrement = range / 256;
                var normalized = this.Take(1).First();
                this.RemoveRange(0, bytes);
                return normalized * unitIncrement + minRange;
            }
            if (bytes == 2)
            {
                var unitIncrement = range / 65536;
                var normalized = BitConverter.ToUInt16(this.Take(2).ToArray(), 0);
                this.RemoveRange(0, bytes);
                return normalized * unitIncrement + minRange;
            }
            if (bytes == 3)
            {
                var unitIncrement = range / 16777216;
                var normalized = BitConverter.ToUInt32(this.Take(2).ToArray(), 0);
                this.RemoveRange(0, bytes);
                return normalized * unitIncrement + minRange;
            }
            throw new Exception("GetCompressedSingle(), bytes must be > 0");
        }

        public void Enqueue(byte value) => Add(value);
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
    }
}
