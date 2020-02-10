using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class ByteArrayExtension
    {
        public static string ToHexString(this byte[] bytes)
        {
            var output = "";
            
            foreach (var @byte in bytes)
                output += $"{@byte:x2}";

            return output;
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
            return value.ToByteHexString() + Encoding.UTF8.GetBytes(value);
        }
    }
}
