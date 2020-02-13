using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityMB : ComponentMB
    {
        public UnityVector3 value;
        public Velocity velocity;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            if (velocity == null) velocity = new Velocity();
            entity.AddComponent(velocity);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(velocity);
        }

        private void Update()
        {
            value.x = velocity.X = transform.position.x;
            value.y = velocity.Y = transform.position.y;
            value.z = velocity.Z = transform.position.z;
        }
    }
}