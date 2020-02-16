using UnityEngine;

#pragma warning disable CA2235 // Mark all non-serializable fields
namespace EntityComponentState.Unity
{
        [System.Serializable]
        public class StateClone
        {
            public GameObject gameObject;
            public int entityId;

            private Rigidbody rigidbody;
            public Transform Transform => gameObject != null ? gameObject.transform : null;
            public Rigidbody Rigidbody => rigidbody != null ? rigidbody : rigidbody = gameObject != null ? gameObject.GetComponent<Rigidbody>() : null;
        }
}
#pragma warning restore CA2235 // Mark all non-serializable fields
