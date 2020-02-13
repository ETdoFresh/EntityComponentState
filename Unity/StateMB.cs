using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateMB : MonoBehaviour
    {
        public State state;
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
            if (state != null)
            {
                var startState = state;
                GetState();
                var deltaState = new DeltaState(startState, state);
                deltaStateString = deltaState.ToString();
                deltaStateBytes = deltaState.ToCompressedByteHexString();
            }
            else
                GetState();
        }

        public void GetState()
        {
            state = new State();
            state.tick = tick;
            state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity.Clone()).OrderBy(e => e.id));
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