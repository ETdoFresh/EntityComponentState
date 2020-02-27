using UnityEngine;

namespace EntityComponentState
{
    [RequireComponent(typeof(StateMB))]
    public class StateHistoryMB : MonoBehaviour
    {
        public StateMB stateMB;
        public StateHistory stateHistory = new StateHistory();
        public DeltaStateHistory deltaStateHistory = new DeltaStateHistory();

        private void OnValidate()
        {
            if (!stateMB) stateMB = GetComponent<StateMB>();
        }

        private void FixedUpdate()
        {
            stateHistory.Add(stateMB.state.Clone());
            deltaStateHistory.Add(stateMB.deltaState.Clone());
        }
    }
}
