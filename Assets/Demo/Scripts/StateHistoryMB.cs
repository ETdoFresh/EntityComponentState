using UnityEngine;

namespace EntityComponentState
{
    [RequireComponent(typeof(StateMB))]
    public class StateHistoryMB : MonoBehaviour
    {
        public StateMB stateMB;
        public StateHistory stateHistory = new StateHistory();

        private void OnValidate()
        {
            if (!stateMB) stateMB = GetComponent<StateMB>();
        }

        private void Update()
        {
            stateHistory.Add(stateMB.state.Clone());
        }
    }
}
