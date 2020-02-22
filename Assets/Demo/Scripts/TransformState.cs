using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Linq;
using UnityEngine;

public class TransformState : MonoBehaviour
{
    public State state = new TState();
    [TextArea(2,20)] public string stateString;
    [TextArea(2,20)] public string stateBytes;

    private void OnEnable()
    {
        state.entities.Clear();
        state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity));
        stateString = state.ToString();
        stateBytes = state.ToBytes().ToHexString();
    }

    public class TState : State
    {
        public static Type[] TYPES = new[]
        {
            typeof(CompressedPosition),
            typeof(CompressedRotation),
            typeof(CompressedScale),
            typeof(Name),
            typeof(Primitive)
        };

        public TState()
        {
            entities = new SerializableListByteCount<Entity>();
            types = TYPES;
        }
    }
}
