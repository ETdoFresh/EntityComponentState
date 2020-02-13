namespace EntityComponentState
{
    public class Name : Component
    {
        public string name;

        public override Component Clone()
        {
            var newComponent = new Name();
            newComponent.entity = entity;
            newComponent.name = name;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            name = ((Name)component).name;
        }

        public override string ToByteHexString()
        {
            return name.ToByteHexString();
        }

        public override string ToCompressedByteHexString()
        {
            return name.ToByteHexString();
        }

        public override byte[] ToBytes()
        {
            return name.ToBytes();
        }

        public override byte[] ToCompressedBytes()
        {
            return name.ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            name = byteQueue.GetString();
        }

        public override string ToString()
        {
            return $"Entity ID: {entity.id} [{name}]";
        }

        public override bool Equals(object obj)
        {
            if (obj is Name other)
                return name == other.name;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
