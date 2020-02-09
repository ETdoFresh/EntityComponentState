using System;

namespace ConsoleApp1
{
    public abstract class Vector3Component : Component
    {
        public Vector3 value;
        public float X { get => value.x; set => this.value.x = value; }
        public float Y { get => value.y; set => this.value.y = value; }
        public float Z { get => value.z; set => this.value.z = value; }

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as Vector3Component;
            newComponent.X = X;
            newComponent.Y = Y;
            newComponent.Z = Z;
            return newComponent;
        }

        public override string ToString()
        {
            return $"Entity ID: {entity.id} [X: {X}, Y: {Y}, Z: {Z}]";
        }
    }
}
