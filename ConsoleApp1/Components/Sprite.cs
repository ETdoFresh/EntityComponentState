using System;

namespace ConsoleApp1
{
    public class Sprite : Component 
    { 
        public string spriteName;

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as Sprite;
            newComponent.spriteName = spriteName;
            return newComponent;
        }
    }
}
