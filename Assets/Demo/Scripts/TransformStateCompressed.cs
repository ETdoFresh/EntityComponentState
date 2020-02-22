using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Linq;
using UnityEngine;

public class TransformStateCompressed : AState
{
    [TextArea(2, 20)] public string stateString;
    [TextArea(2, 20)] public string stateBytes;

    private void OnEnable()
    {
        state = new TransformState();
    }

    private void Update()
    {
        state.entities.Clear();
        state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity));
        Entity.AS_BYTE = true;
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";
    }

    public override ByteQueue ToBytes()
    {
        Entity.AS_BYTE = true;
        return state.ToBytes();
    }

    public override void FromBytes(ByteQueue bytes)
    {
        Entity.AS_BYTE = true;
        state.FromBytes(bytes);
        stateString = state.ToString();
        stateBytes = $"{state.ToBytes().ToHexString()} [{state.ToBytes().Count}]";
    }

    public class TransformState : State
    {
        public static Type[] TYPES = new[]
        {
            typeof(CompressedPosition),
            typeof(CompressedRotation),
            typeof(CompressedScale),
            typeof(Name),
            typeof(Primitive)
        };

        public TransformState()
        {
            entities = new SerializableListByteCount<Entity>();
            types = TYPES;
        }
    }
}
