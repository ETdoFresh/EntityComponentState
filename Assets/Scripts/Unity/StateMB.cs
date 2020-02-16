using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateMB : MonoBehaviour
    {
        public State state;
        public DeltaState deltaState;
        public int tick;
        [Multiline(20)] public string stateString;
        [Multiline(3)] public string stateBytes;
        [Multiline(40)] public string deltaStateString;
        [Multiline(3)] public string deltaStateBytes;

        private void Update()
        {
            tick++;
            GetDeltaState();
        }

        public void GetDeltaState()
        {
            var startState = state != null ? state : new State();
            GetState();
            deltaState = new DeltaState(startState, state);

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
            stateBytes = state.ToCompressedByteHexString();
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