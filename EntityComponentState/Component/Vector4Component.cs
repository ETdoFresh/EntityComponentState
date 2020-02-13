using System;
using System.Collections.Generic;

namespace EntityComponentState
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
            newComponent.value = value;
            return newComponent;
        }

        public override void CopyValuesFrom(Component component)
        {
            value = ((Vector4Component)component).value;
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
            var output = X.ToByteHexString();
            output += $" {Y.ToByteHexString()}";
            output += $" {Z.ToByteHexString()}";
            output += $" {W.ToByteHexString()}";
            return output;
        }

        public override string ToCompressedByteHexString()
        {
            var output = X.ToCompressedBytesHexString(-1.28f, 1.28f, 1);
            output += $" {Y.ToCompressedBytesHexString(-1.28f, 1.28f, 1)}";
            output += $" {Z.ToCompressedBytesHexString(-1.28f, 1.28f, 1)}";
            output += $" {W.ToCompressedBytesHexString(-1.28f, 1.28f, 1)}";
            return output;
        }

        public override byte[] ToBytes()
        {
            var output = new List<byte>();
            output.AddRange(X.ToBytes());
            output.AddRange(Y.ToBytes());
            output.AddRange(Z.ToBytes());
            output.AddRange(W.ToBytes());
            return output.ToArray();
        }

        public override byte[] ToCompressedBytes()
        {
            var output = new List<byte>();
            output.AddRange(X.ToCompressedBytes(-1.28f, 1.28f, 1));
            output.AddRange(Y.ToCompressedBytes(-1.28f, 1.28f, 1));
            output.AddRange(Z.ToCompressedBytes(-1.28f, 1.28f, 1));
            output.AddRange(W.ToCompressedBytes(-1.28f, 1.28f, 1));
            return output.ToArray();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            X = byteQueue.GetSingle();
            Y = byteQueue.GetSingle();
            Z = byteQueue.GetSingle();
            W = byteQueue.GetSingle();
        }
    }
}
