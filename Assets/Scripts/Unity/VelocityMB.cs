using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityMB : ComponentMB<Velocity>
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
            value.x = component.X = rigidbody.velocity.x;
            value.y = component.Y = rigidbody.velocity.y;
            value.z = component.Z = rigidbody.velocity.z;
        }
    }
}