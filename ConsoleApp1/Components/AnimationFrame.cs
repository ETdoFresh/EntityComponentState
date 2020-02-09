using System;

namespace ConsoleApp1
{
    public class AnimationFrame : Component 
    {
        public int frame;

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as AnimationFrame;
            newComponent.frame = frame;
            return newComponent;
        }
    }
}
