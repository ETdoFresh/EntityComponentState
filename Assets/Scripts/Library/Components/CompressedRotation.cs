namespace EntityComponentState
{
    public class CompressedRotation : Vector4Component
    {
        private const float MIN_ROTATION = -1.28f;
        private const float MAX_ROTATION = 1.28f;

        public override ByteQueue ToBytes()
        {
            var output = new ByteQueue();
            output.Enqueue(Compression.MapToByte(X, MIN_ROTATION, MAX_ROTATION));
            output.Enqueue(Compression.MapToByte(Y, MIN_ROTATION, MAX_ROTATION));
            output.Enqueue(Compression.MapToByte(Z, MIN_ROTATION, MAX_ROTATION));
            output.Enqueue(Compression.MapToByte(W, MIN_ROTATION, MAX_ROTATION));
            return output;
        }

        public override void FromBytes(ByteQueue bytes)
        {
            X = Compression.MapFromByte(bytes.GetByte(), MIN_ROTATION, MAX_ROTATION);
            Y = Compression.MapFromByte(bytes.GetByte(), MIN_ROTATION, MAX_ROTATION);
            Z = Compression.MapFromByte(bytes.GetByte(), MIN_ROTATION, MAX_ROTATION);
            W = Compression.MapFromByte(bytes.GetByte(), MIN_ROTATION, MAX_ROTATION);
        }
    }
}
