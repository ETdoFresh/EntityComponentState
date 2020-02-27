namespace EntityComponentState
{
    public class LongValue : IToBytes
    {
        public long value;

        public virtual void FromBytes(ByteQueue bytes) => value = bytes.GetLong();
        public virtual ByteQueue ToBytes() => new ByteQueue(value);
    }

    public class IntValue : LongValue
    {
        public override void FromBytes(ByteQueue bytes) => value = bytes.GetInt();
        public override ByteQueue ToBytes() => new ByteQueue((int)value);
    }

    public class ShortValue : LongValue
    {
        public override void FromBytes(ByteQueue bytes) => value = bytes.GetShort();
        public override ByteQueue ToBytes() => new ByteQueue((short)value);
    }

    public class ByteValue : IntValue
    {
        public override void FromBytes(ByteQueue bytes) => value = bytes.GetByte();
        public override ByteQueue ToBytes() => new ByteQueue((byte)value);
    }
}