using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class AngularVelocityMB : ComponentMB<AngularVelocity>
    {
        public UnityVector3 value;
        public new Rigidbody rigidbody;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!rigidbody) rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            value.x = component.X = rigidbody.angularVelocity.x;
            value.y = component.Y = rigidbody.angularVelocity.y;
            value.z = component.Z = rigidbody.angularVelocity.z;
        }
    }
}