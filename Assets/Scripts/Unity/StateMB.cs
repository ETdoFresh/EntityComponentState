using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateMB : MonoBehaviour
    {
        public State state;
        public DeltaStateOld deltaState;
        public int tick;
        [TextArea(1, 20)] public string stateString;
        [TextArea(1, 20)] public string stateBytes;
        [TextArea(1, 20)] public string deltaStateString;
        [TextArea(1, 20)] public string deltaStateBytes;

        private void Update()
        {
            tick++;
            GetDeltaState();
        }

        public void GetDeltaState()
        {
            var startState = state != null ? state : new State();
            GetState();
            deltaState = new DeltaStateOld(startState, state);

            // Create lots of garbage for GC
            deltaStateString = deltaState.ToString();
            deltaStateBytes = deltaState.ToCompressedByteHexString();
        }

        public void GetState()
        {
            state = new State { tick = tick };
            state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity.Clone()).OrderBy(e => e.id));

            // Create lots of garbage for GC
            stateString = state.ToString();
            stateBytes = state.ToBytes().ToHexString();
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(StateMB))]
    public class StateMBEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Get Current State"))
                ((StateMB)target).GetState();
        }
    }
#endif
}