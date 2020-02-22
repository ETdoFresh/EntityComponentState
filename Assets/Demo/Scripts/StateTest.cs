using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateTest : MonoBehaviour
    {
        public StateMB stateMB;
        public State beforeSerializeState;
        public State afterDeserializeState;
        [TextArea(2, 8)] public string bytesString;

        private void OnEnable()
        {
            beforeSerializeState = stateMB.state.Clone();
            var bytes = beforeSerializeState.ToBytes();
            bytesString = bytes.ToHexString();
            afterDeserializeState = new State();
            afterDeserializeState.FromBytes(bytes);
        }
    }
}