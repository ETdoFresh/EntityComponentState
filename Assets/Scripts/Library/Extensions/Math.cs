using System;
using System.Collections.Generic;
using System.Text;
using MathSys = System.Math;

namespace EntityComponentState
{
    public static class Math
    {
        public static float Clamp(float value, float min, float max)
        {
            return MathSys.Max(MathSys.Min(value, max), min);
        }

        public static float Ceiling(float value)
        {
            return (float)MathSys.Ceiling(value);
        }

        public static int CeilingToInt(float value)
        {
            return Convert.ToInt32(MathSys.Ceiling(value));
        }

        public static uint CeilingToUInt(float value)
        {
            return Convert.ToUInt32(MathSys.Ceiling(value));
        }

        public static ushort CeilingToUShort(float value)
        {
            return Convert.ToUInt16(MathSys.Ceiling(value));
        }

        public static byte CeilingToByte(float value)
        {
            return Convert.ToByte(MathSys.Ceiling(value));
        }

        public static int Min(int val1, int val2) => MathSys.Min(val1, val2);
    }
}
