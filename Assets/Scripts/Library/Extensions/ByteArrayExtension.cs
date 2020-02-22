using System;
using System.Linq;
using System.Text;

namespace EntityComponentState
{
    public static partial class ByteArrayExtension
    {
        public static string ToHexString(this byte @byte)
        {
            return $"{@byte:x2}";
        }

        public static string ToHexString(this byte[] bytes)
        {
            var output = "";

            foreach (var @byte in bytes)
                output += $"{@byte:x2}";

            return output;
        }

        public static string ToByteHexString(this byte value)
        {
            return value.ToHexString();
        }

        public static string ToByteHexString(this int value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this long value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this short value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this char value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this float value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this double value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this bool value)
        {
            return BitConverter.GetBytes(value).ToHexString();
        }

        public static string ToByteHexString(this string value)
        {
            return value.Length.ToByteHexString() + Encoding.UTF8.GetBytes(value).ToHexString();
        }

        public static byte[] ToBytes(this byte value)
        {
            return new byte[] { value };
        }

        public static byte[] ToBytes(this int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this string value)
        {
            return value.Length.ToBytes().Concat(Encoding.UTF8.GetBytes(value)).ToArray();
        }

        public static byte[] ToBytes(this float value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToCompressedBytes(this float value, float minRange, float maxRange, int bytes)
        {
            if (bytes >= 4) return ToBytes(value);
            value = Math.Clamp(value, minRange, maxRange);
            var range = maxRange - minRange;
            var normalized = (value - minRange) / range;
            if (bytes == 1)
            {
                var unitIncrement = range / 256;
                var incrementValue = Math.CeilingToByte(normalized / unitIncrement);
                return BitConverter.GetBytes(incrementValue);
            }
            if (bytes == 2)
            {
                var unitIncrement = range / 65536;
                var incrementValue = (ushort)Math.CeilingToUShort(normalized / unitIncrement);
                return BitConverter.GetBytes(incrementValue);
            }
            if (bytes == 3)
            {
                var unitIncrement = range / 16777216;
                var incrementValue = (uint)Math.CeilingToUInt(normalized / unitIncrement);
                return BitConverter.GetBytes(incrementValue).Take(3).ToArray();
            }
            throw new Exception("GetCompressedSingle(), bytes must be > 0");
        }

        public static string ToCompressedBytesHexString(this float value, float minRange, float maxRange, int bytes)
        {
            return ToCompressedBytes(value, minRange, maxRange, bytes).ToHexString();
        }
    }
}
