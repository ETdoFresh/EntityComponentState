using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public abstract class Component : IToBytes
    {
        private static Type Type => typeof(Component);
        private static IEnumerable<Type> allTypes;
        public static IEnumerable<Type> ALL_TYPES
        {
            get
            {
                if (allTypes == null)
                    return (allTypes = Type.Assembly.GetTypes().Where(otherType => Type.IsAssignableFrom(otherType)));
                else
                    return allTypes;
            }
        }

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

        public abstract ByteQueue ToBytes();

        public abstract void FromBytes(ByteQueue bytes);

        public abstract byte[] ToCompressedBytes();

        public abstract void CopyValuesFrom(Component component);

        public abstract Component Lerp(Component endComponent, float t);

        public static Component Lerp(Component startComponent, Component endComponent, float t)
        {
            return startComponent.Lerp(endComponent, t);
        }
    }
}
