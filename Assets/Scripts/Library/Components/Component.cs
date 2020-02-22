using EntityComponentState;
using System;
using System.Collections.Generic;

namespace EntityComponentState
{
    public abstract class Component : IToBytes
    {
        public static List<Type> types = new List<Type>
        {
            typeof(Position),
            typeof(Rotation),
            typeof(Scale),
            typeof(Velocity),
            typeof(AngularVelocity),
            typeof(Sprite),
            typeof(AnimationFrame),
            typeof(Primitive),
            typeof(Name)
        };

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
    }
}
