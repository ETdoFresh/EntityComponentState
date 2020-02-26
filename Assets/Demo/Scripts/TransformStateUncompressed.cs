using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformStateUncompressed : StateMB
{
    public override State state { get; protected set; } = new TransformState();
    //public override DeltaState deltaState { get; protected set; } = new TransformDeltaState();

    [TextArea(2, 20)] public string stateString;
    [TextArea(2, 20)] public string stateBytes;

    private void Update()
    {
        state.tick++;
        state.entities.Clear();
        state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity));
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";
    }

    public override ByteQueue ToBytes()
    {
        return state.ToBytes();
    }

    public override void FromBytes(ByteQueue bytes)
    {
        state.FromBytes(bytes);
    }

    public class TransformState : State
    {
        public static Type[] TYPES = new[]
{
            typeof(Position),
            typeof(Rotation),
            typeof(Scale),
            typeof(Name),
            typeof(Primitive)
        };
        public override IEnumerable<Type> types { get; protected set; } = TYPES;
    }
}