using System;

namespace EntityComponentState
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

        public override ByteQueue ToBytes()
        {
            return new ByteQueue(frame);
        }

        public override void FromBytes(ByteQueue bytes)
        {
            frame = bytes.GetInt();
        }

        public override byte[] ToCompressedBytes()
        {
            return ((byte)frame).ToBytes();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            frame = byteQueue.GetInt();
        }

        public override string ToString()
        {
            return $"[{entity.id}][{frame}][{GetType().Name}]";
        }

        public override Component Lerp(Component endComponent, float t)
        {
            var start = this;
            var end = (AnimationFrame)endComponent;
            var range = end.frame - start.frame;
            var lerp = (AnimationFrame)Clone();
            lerp.frame += Math.RoundToInt(t * range);
            return lerp;
        }
    }
}
