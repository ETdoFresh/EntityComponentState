using System;

namespace ConsoleApp1
{
    public class AnimationFrame : Component 
    {
        public int frame;

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as AnimationFrame;
            newComponent.entity = entity;
            newComponent.frame = frame;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            frame = ((AnimationFrame)component).frame;
        }

        public override string ToByteHexString()
        {
            return frame.ToByteHexString();
        }

        public override string ToCompressedByteHexString()
        {
            return ((byte)frame).ToByteHexString();
        }

        public override byte[] ToBytes()
        {
            return frame.ToBytes();
        }

        public override byte[] ToCompressedBytes()
        {
            return ((byte)frame).ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            frame = byteQueue.GetInt32();
        }
    }
}
