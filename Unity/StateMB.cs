using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateMB : MonoBehaviour
    {
        public State state;
        [Multiline(35)] public string humanReadable;

        public void CaptureState()
        {
            state = new State();
            state.entities.AddRange(FindObjectsOfType<EntityMB>().Select(e => e.entity).OrderBy(e => e.id));
            humanReadable = state.ToString();
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
                ((StateMB)target).CaptureState();
        }
    }
#endif
}