namespace EntityComponentState
{
    public class Primitive : Component
    {
        public enum PrimitiveType { Sphere, Capsule, Cylinder, Cube, Plane, Quad }

        public PrimitiveType primitiveType;

        public override Component Clone()
        {
            var newComponent = new Primitive();
            newComponent.entity = entity;
            newComponent.primitiveType = primitiveType;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            primitiveType = ((Primitive)component).primitiveType;
        }

        public override string ToByteHexString()
        {
            return ((byte)primitiveType).ToByteHexString();
        }

        public override string ToCompressedByteHexString()
        {
            return ((byte)primitiveType).ToByteHexString();
        }

        public override ByteQueue ToBytes()
        {
            return new ByteQueue((byte)primitiveType);
        }

        public override void FromBytes(ByteQueue bytes)
        {
            primitiveType = (PrimitiveType)bytes.GetByte();
        }

        public override byte[] ToCompressedBytes()
        {
            return ((byte)primitiveType).ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            primitiveType = ((PrimitiveType)byteQueue.GetByte());
        }

        public override string ToString()
        {
            return $"[{entity.id}][{primitiveType.ToString()}][{GetType().Name}]";
        }

        public override bool Equals(object obj)
        {
            if (obj is Primitive other)
                return primitiveType == other.primitiveType;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return primitiveType.GetHashCode();
        }

        public override Component Lerp(Component endComponent, float t)
        {
            return t < 1 ? this : endComponent;
        }
    }
}
