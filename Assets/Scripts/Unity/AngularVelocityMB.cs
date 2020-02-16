using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class AngularVelocityMB : ComponentMB
    {
        public UnityVector3 value;
        public AngularVelocity angularVelocity;
        public new Rigidbody rigidbody;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            rigidbody = GetComponent<Rigidbody>();

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
            value.x = angularVelocity.X = rigidbody.angularVelocity.x;
            value.y = angularVelocity.Y = rigidbody.angularVelocity.y;
            value.z = angularVelocity.Z = rigidbody.angularVelocity.z;
        }
    }
}