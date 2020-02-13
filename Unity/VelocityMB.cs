using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityMB : ComponentMB
    {
        public UnityVector3 value;
        public Velocity velocity;
        public Rigidbody rigidbody;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            rigidbody = GetComponent<Rigidbody>();
            if (velocity == null) velocity = new Velocity();
            entity.AddComponent(velocity);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(velocity);
        }

        private void Update()
        {
            value.x = velocity.X = rigidbody.velocity.x;
            value.y = velocity.Y = rigidbody.velocity.y;
            value.z = velocity.Z = rigidbody.velocity.z;
        }
    }
}