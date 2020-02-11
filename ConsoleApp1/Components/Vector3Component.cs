using System;
using System.Collections.Generic;

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
            newComponent.entity = entity;
            newComponent.value = value;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            value = ((Vector3Component)component).value;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                if (obj is Vector3Component other)
                    if (this.value == other.value)
                        return true;

            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Vector3Component lhs, Vector3Component rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector3Component lhs, Vector3Component rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"Entity ID: {entity.id} [X: {X}, Y: {Y}, Z: {Z}]";
        }

        public override string ToByteHexString()
        {
            var output = X.ToByteHexString();
            output += $" {Y.ToByteHexString()}";
            output += $" {Z.ToByteHexString()}";
            return output;
        }

        public override byte[] ToBytes()
        {
            var output = new List<byte>();
            output.AddRange(X.ToBytes());
            output.AddRange(Y.ToBytes());
            output.AddRange(Z.ToBytes());
            return output.ToArray();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            X = byteQueue.GetSingle();
            Y = byteQueue.GetSingle();
            Z = byteQueue.GetSingle();
        }
    }
}
