using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class ByteQueue : List<byte>
    {
        public ByteQueue(byte[] bytes) : base(bytes) { }

        public int GetInt32()
        {
            var size = 4;
            var value = BitConverter.ToInt32(this.Take(size).ToArray(), 0);
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

        public string GetString()
        {
            var count = BitConverter.ToInt32(this.Take(4).ToArray(), 0);
            var value = Encoding.UTF8.GetString(this.Skip(4).Take(count).ToArray());
            RemoveRange(0, 4 + count);
            return value;
        }

        public float GetSingle()
        {
            var size = 4;
            var value = BitConverter.ToSingle(this.Take(size).ToArray(), 0);
            RemoveRange(0, size);
            return value;
        }
    }
}
