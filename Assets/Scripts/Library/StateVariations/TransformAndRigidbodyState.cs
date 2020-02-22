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