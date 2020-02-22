namespace EntityComponentState
{
    public interface IToBytes
    {
        ByteQueue ToBytes();
        void FromBytes(ByteQueue bytes);
    }
}