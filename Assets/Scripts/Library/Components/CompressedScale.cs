namespace EntityComponentState
{
    public class CompressedScale : Vector3Component
    {
        private const float MIN_POSITION = 0f;
        private const float MAX_POSITION = 64f;

        public override ByteQueue ToBytes()
        {
            var output = new ByteQueue();
            output.Enqueue(Compression.MapToByte(X, MIN_POSITION, MAX_POSITION));
            output.Enqueue(Compression.MapToByte(Y, MIN_POSITION, MAX_POSITION));
            output.Enqueue(Compression.MapToByte(Z, MIN_POSITION, MAX_POSITION));
            return output;
        }

        public override void FromBytes(ByteQueue bytes)
        {
            X = Compression.MapFromByte(bytes.GetByte(), MIN_POSITION, MAX_POSITION);
            Y = Compression.MapFromByte(bytes.GetByte(), MIN_POSITION, MAX_POSITION);
            Z = Compression.MapFromByte(bytes.GetByte(), MIN_POSITION, MAX_POSITION);
        }
    }
}
