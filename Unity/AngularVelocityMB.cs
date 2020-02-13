using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class AngularVelocityMB : ComponentMB
    {
        public UnityVector3 value;
        public AngularVelocity angularVelocity;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;

            if (entity.HasComponent<AngularVelocity>())
                angularVelocity = entity.GetComponent<AngularVelocity>();
            else if (angularVelocity == null)
                angularVelocity = new AngularVelocity();

            if (!entity.HasComponent<AngularVelocity>())
                entity.AddComponent(angularVelocity);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(angularVelocity);
        }

        private void Update()
        {
            value.x = angularVelocity.X = transform.position.x;
            value.y = angularVelocity.Y = transform.position.y;
            value.z = angularVelocity.Z = transform.position.z;
        }
    }
}