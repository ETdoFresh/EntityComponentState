﻿using System;
using System.Collections.Generic;

namespace EntityComponentState
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
            return $"[{entity.id}][X: {X}, Y: {Y}, Z: {Z}][{GetType().Name}]";
        }

        public override string ToByteHexString()
        {
            var output = X.ToByteHexString();
            output += $" {Y.ToByteHexString()}";
            output += $" {Z.ToByteHexString()}";
            return output;
        }

        public override string ToCompressedByteHexString()
        {
            var output = X.ToCompressedBytesHexString(-65.536f, 65.536f, 2);
            output += $" {Y.ToCompressedBytesHexString(-65.536f, 65.536f, 2)}";
            output += $" {Z.ToCompressedBytesHexString(-65.536f, 65.536f, 2)}";
            return output;
        }

        public override ByteQueue ToBytes()
        {
            var output = new ByteQueue();
            output.Enqueue(X);
            output.Enqueue(Y);
            output.Enqueue(Z);
            return output;
        }

        public override void FromBytes(ByteQueue bytes)
        {
            X = bytes.GetFloat();
            Y = bytes.GetFloat();
            Z = bytes.GetFloat();
        }

        public override byte[] ToCompressedBytes()
        {
            var output = new List<byte>();
            output.AddRange(X.ToCompressedBytes(-65.536f, 65.536f, 2));
            output.AddRange(Y.ToCompressedBytes(-65.536f, 65.536f, 2));
            output.AddRange(Z.ToCompressedBytes(-65.536f, 65.536f, 2));
            return output.ToArray();
        }

        public override void Deserialize(ByteQueue byteQueue)
        {
            X = byteQueue.GetFloat();
            Y = byteQueue.GetFloat();
            Z = byteQueue.GetFloat();
        }

        public override Component Lerp(Component endComponent, float t)
        {
            var start = this;
            var end = (Vector3Component)endComponent;
            if (start.value == end.value) return this;

            var rangeX = end.X - X;
            var rangeY = end.Y - Y;
            var rangeZ = end.Z - Z;
            var lerp = (Vector3Component)Clone();
            lerp.X += rangeX * t;
            lerp.Y += rangeY * t;
            lerp.Z += rangeZ * t;
            return lerp;
        }
    }
}
