namespace EntityComponentState
{
    public static class Compression
    {
        public static byte MapToByte(float value, float min, float max)
        {
            var range = max - min;
            var normalized = (value - min) / range;
            return (byte)Math.Round(normalized * 256);
        }

        public static float MapFromByte(byte value, float min, float max)
        {
            var normalized = value / 256f;
            var range = max - min;
            return normalized * range + min;
        }

        public static ushort MapToUShort(float value, float min, float max)
        {
            var range = max - min;
            var normalized = (value - min) / range;
            return (ushort)Math.Round(normalized * 65536);
        }

        public static float MapFromUShort(ushort value, float min, float max)
        {
            var normalized = value / 65536f;
            var range = max - min;
            return normalized * range + min;
        }
    }
}