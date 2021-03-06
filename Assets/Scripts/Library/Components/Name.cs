﻿namespace EntityComponentState
{
    public class Name : Component
    {
        public string name = "";

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

        public override ByteQueue ToBytes()
        {
            return new ByteQueue(name);
        }

        public override void FromBytes(ByteQueue bytes)
        {
            name = bytes.GetString();
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
            return $"[{entity.id}][{name}][{GetType().Name}]";
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

        public override Component Lerp(Component endComponent, float t)
        {
            return t < 1 ? this : endComponent;
        }
    }
}
