using System;

namespace ConsoleApp1
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

        public override byte[] ToBytes()
        {
            return spriteName.ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            spriteName = byteQueue.GetString();
        }
    }
}
