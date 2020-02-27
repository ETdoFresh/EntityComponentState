using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformStateCompressed : StateMB
{
    public override State state { get; protected set; } = new TransformState();
    public override DeltaState deltaState { get; protected set; } = new TransformDeltaState();
    private State previousState = new TransformState();

    [TextArea(2, 20)] public string stateString;
    [TextArea(2, 20)] public string stateBytes;

    [TextArea(2, 20)] public string deltaStateString;
    [TextArea(2, 20)] public string deltaStateBytes;

    private void FixedUpdate()
    {
        state.tick++;
        state.entities.Clear();
        state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity).OrderBy(e => e.id));
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";

        deltaState.Create(previousState, state);
        deltaStateString = deltaState.ToString();
        var deltaStateBytes = deltaState.ToBytes();
        this.deltaStateBytes = $"{deltaStateBytes.ToHexString()} [{deltaStateBytes.Count}]";

        previousState.tick = state.tick;
        previousState.entities.Clear();
        previousState.entities.AddRange(state.entities.Clone());
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
        
        public override IEnumerable<Type> componentTypes => TYPES;
        public override Type deltaType => typeof(TransformDeltaState);
    }

    public class TransformDeltaState : CompressedDeltaState
    {
        public override IEnumerable<Type> componentTypes => TransformState.TYPES;
        public override Type stateType => typeof(TransformState);
        
        public TransformDeltaState() { }
        public TransformDeltaState(State startState, State endState) : base(startState, endState) { }
    }
}
