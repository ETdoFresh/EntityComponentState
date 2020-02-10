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

        public override string ToByteHexString()
        {
            var output = base.ToByteHexString();
            output += $" {frame.ToByteHexString()}";
            return output;
        }
    }
}
