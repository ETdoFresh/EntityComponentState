using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformStateCompressed : AState
{
    public override State state { get; protected set; } = new TransformState();
    public override DeltaState deltaState { get; protected set; } = new TransformDeltaState();
    private State previousState = new TransformState();

    [TextArea(2, 20)] public string stateString;
    [TextArea(2, 20)] public string stateBytes;

    [TextArea(2, 20)] public string deltaStateString;
    [TextArea(2, 20)] public string deltaStateBytes;

    private void Update()
    {
        state.tick++;
        state.entities.Clear();
        state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity));
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";

        deltaState.Create(previousState, state);
        deltaStateString = deltaState.ToString();
        deltaStateBytes = $"{deltaState.ToBytes().ToHexString()} [{deltaState.ToBytes().Count}]";

        previousState.tick = state.tick;
        previousState.entities.Clear();
        previousState.entities.AddRange(state.entities.Clone());
    }

    public override ByteQueue ToBytes()
    {
        return state.ToBytes();
    }

    public override void FromBytes(ByteQueue bytes)
    {
        state.FromBytes(bytes);
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";
    }

    public class TransformState : CompressedState
    {
        public static Type[] TYPES = new[]
        {
            typeof(CompressedPosition),
            typeof(CompressedRotation),
            typeof(CompressedScale),
            typeof(Name),
            typeof(Primitive)
        };
        public override IEnumerable<Type> types { get; protected set; } = TYPES;
    }

    public class TransformDeltaState : DeltaState
    {
        public TransformDeltaState() { }
        public TransformDeltaState(State startState, State endState) : base(startState, endState) { }
        public override State startState { get; protected set; } = new TransformState();
        public override State endState { get; protected set; } = new TransformState();
    }
}
