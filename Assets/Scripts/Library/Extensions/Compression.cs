using UnityEngine;

public static class Compression
{
    public static float MapToByte(float value, float min, float max)
    {
        var range = max - min;
        var normalized = (value - min) / range;
        return (byte)Mathf.Round(normalized * 256);
    }

    public static float MapFromByte(byte value, float min, float max)
    {
        var range = max - min;
        return value / range + min;
    }

    public static float MapToUShort(float value, float min, float max)
    {
        var range = max - min;
        var normalized = (value - min) / range;
        return (ushort)Mathf.Round(normalized * 65536);
    }

    public static float MapFromUShort(ushort value, float min, float max)
    {
        var range = max - min;
        return value / range + min;
    }
}
