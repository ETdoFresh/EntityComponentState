using System;

namespace ConsoleApp1
{
    public abstract class Vector4Component : Component
    {
        public Vector4 value;
        public float X { get => value.x; set => this.value.x = value; }
        public float Y { get => value.y; set => this.value.y = value; }
        public float Z { get => value.z; set => this.value.z = value; }
        public float W { get => value.w; set => this.value.w = value; }

        public override Component Clone()
        {
            var newComponent = Activator.CreateInstance(GetType()) as Vector4Component;
            newComponent.entity = entity;
            newComponent.X = X;
            newComponent.Y = Y;
            newComponent.Z = Z;
            newComponent.W = W;
            return newComponent;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                if (obj is Vector4Component other)
                    if (this.value == other.value)
                        return true;

            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Vector4Component lhs, Vector4Component rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector4Component lhs, Vector4Component rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"Entity ID: {entity.id} [X: {X}, Y: {Y}, Z: {Z}, W: {W}]";
        }

        public override string ToByteHexString()
        {
            var output = base.ToByteHexString();
            output += $" {X.ToByteHexString()}";
            output += $" {Y.ToByteHexString()}";
            output += $" {Z.ToByteHexString()}";
            output += $" {W.ToByteHexString()}";
            return output;
        }
    }
}
