using EntityComponentState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAndRigidbodyState : State
{
    public SerializableList<Position> positions;
    public SerializableList<Rotation> rotations;
    public SerializableList<Scale> scales;
    public SerializableList<Velocity> velocities;
    public SerializableList<AngularVelocity> angularVelocities;
}

public interface IToBytes
{
    ByteQueue ToBytes();
    void FromBytes(ByteQueue bytes);
}

public class SerializableList<T> : List<T>, IToBytes where T : IToBytes
{
    public virtual ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        foreach (var item in this)
            bytes.Enqueue(item);
        return bytes;
    }

    public virtual void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            this[i].FromBytes(bytes);
    }
}

public class SerializableListByteCount<T> : SerializableList<T> where T : IToBytes
{
    public override ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(((byte)Count));
        foreach (var item in this)
            bytes.Enqueue(item);
        return bytes;
    }

    public override void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetByte();
        for (int i = 0; i < count; i++)
            this[i].FromBytes(bytes);
    }
}