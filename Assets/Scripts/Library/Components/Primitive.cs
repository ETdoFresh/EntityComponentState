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

        public override byte[] ToBytes()
        {
            return ((byte)primitiveType).ToBytes();
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
            return $"Entity ID: {entity.id} [{primitiveType.ToString()}]";
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
    }
}
