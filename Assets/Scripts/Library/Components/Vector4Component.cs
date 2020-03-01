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
            return $"[{entity.id}][X: {X}, Y: {Y}, Z: {Z}, W: {W}][{GetType().Name}]";
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

        public override ByteQueue ToBytes()
        {
            var output = new ByteQueue();
            output.Enqueue(X);
            output.Enqueue(Y);
            output.Enqueue(Z);
            output.Enqueue(W);
            return output;
        }

        public override void FromBytes(ByteQueue bytes)
        {
            X = bytes.GetFloat();
            Y = bytes.GetFloat();
            Z = bytes.GetFloat();
            W = bytes.GetFloat();
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
            X = byteQueue.GetFloat();
            Y = byteQueue.GetFloat();
            Z = byteQueue.GetFloat();
            W = byteQueue.GetFloat();
        }

        public override Component Lerp(Component endComponent, float t)
        {
            var start = this;
            var end = (Vector4Component)endComponent;
            if (start.value == end.value) return this;

            var rangeX = end.X - X;
            var rangeY = end.Y - Y;
            var rangeZ = end.Z - Z;
            var rangeW = end.W - W;
            var lerp = (Vector4Component)Clone();
            lerp.X += rangeX * t;
            lerp.Y += rangeY * t;
            lerp.Z += rangeZ * t;
            lerp.W += rangeW * t;
            return lerp;
        }
    }
}
