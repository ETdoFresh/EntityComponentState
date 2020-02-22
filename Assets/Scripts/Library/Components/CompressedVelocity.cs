namespace EntityComponentState
{
    public class CompressedVelocity : Velocity
    {
        private const float MIN_POSITION = -65.536f;
        private const float MAX_POSITION = 65.536f;

        public override ByteQueue ToBytes()
        {
            var output = new ByteQueue();
            output.Enqueue(Compression.MapToUShort(X, MIN_POSITION, MAX_POSITION));
            output.Enqueue(Compression.MapToUShort(Y, MIN_POSITION, MAX_POSITION));
            output.Enqueue(Compression.MapToUShort(Z, MIN_POSITION, MAX_POSITION));
            return output;
        }

        public override void FromBytes(ByteQueue bytes)
        {
            X = Compression.MapFromUShort(bytes.GetUShort(), MIN_POSITION, MAX_POSITION);
            Y = Compression.MapFromUShort(bytes.GetUShort(), MIN_POSITION, MAX_POSITION);
            Z = Compression.MapFromUShort(bytes.GetUShort(), MIN_POSITION, MAX_POSITION);
        }
    }
}
