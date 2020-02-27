using UnityEngine;
using static TransformStateCompressed;

namespace EntityComponentState
{
    [RequireComponent(typeof(StateMB))]
    public class StateHistoryMB : MonoBehaviour
    {
        public StateMB stateMB;
        public StateHistory<TransformState> stateHistory = new StateHistory<TransformState>();
        public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();

        private void OnValidate()
        {
            if (!stateMB) stateMB = GetComponent<StateMB>();
        }

        private void FixedUpdate()
        {
            stateHistory.Add((TransformState)stateMB.state.Clone());
            deltaStateHistory.Add((TransformDeltaState)stateMB.deltaState.Clone());
        }
    }
}
