using EntityComponentState;
using System;

namespace TransformStateLibrary
{
    public static class Packet
    {
        public static byte[] Create(params byte[] bytes)
        {
            return bytes;
        }

        public static byte[] Create(byte @byte, params byte[] bytes)
        {
            var moreBytes = new byte[bytes.Length + 1];
            moreBytes[0] = @byte;
            Array.Copy(bytes, 0, moreBytes, 1, bytes.Length);
            return moreBytes;
        }

        public static byte[] Create(byte @byte, ByteQueue bytes)
        {
            return Create(@byte, bytes.ToArray());
        }

        public static byte[] Create(CommandEnum command, ByteQueue bytes)
        {
            return Create((byte)command, bytes);
        }
    }
}
