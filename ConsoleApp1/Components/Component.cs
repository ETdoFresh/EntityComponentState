using System;

namespace ConsoleApp1
{
    public abstract class Component
    {       
        public Entity entity = Entity.NULL;

        public abstract Component Clone();

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is Component other)
                return this.entity.id == other.entity.id;
            else
                return false;
        }

        public static bool operator ==(Component lhs, Component rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Component lhs, Component rhs)
            => !(lhs == rhs);

        public override int GetHashCode()
        {
            return entity.id;
        }

        public abstract string ToByteHexString();

        public abstract string ToCompressedByteHexString();

        public abstract void Deserialize(ByteQueue byteQueue);

        public abstract byte[] ToBytes();
        
        public abstract byte[] ToCompressedBytes();

        public abstract void CopyValuesFrom(Component component);
    }
}
