using System;

namespace EntityComponentState
{
    public class Sprite : Component 
    { 
        public string spriteName;

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as Sprite;
            newComponent.entity = entity;
            newComponent.spriteName = spriteName;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            spriteName = ((Sprite)component).spriteName;
        }

        public override string ToByteHexString()
        {
            return spriteName.ToByteHexString();
        }

        public override string ToCompressedByteHexString()
        {
            return spriteName.ToByteHexString();
        }

        public override ByteQueue ToBytes()
        {
            return new ByteQueue(spriteName);
        }

        public override void FromBytes(ByteQueue bytes)
        {
            spriteName = bytes.GetString();
        }

        public override byte[] ToCompressedBytes()
        {
            return spriteName.ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            spriteName = byteQueue.GetString();
        }

        public override string ToString()
        {
            return $"[{entity.id}][{spriteName}][{GetType().Name}]";
        }

        public override Component Lerp(Component endComponent, float t)
        {
            return t < 1 ? this : endComponent;
        }
    }
}
