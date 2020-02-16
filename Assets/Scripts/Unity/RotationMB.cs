using UnityEngine;

namespace EntityComponentState.Unity
{
    public class RotationMB : ComponentMB
    {
        public Quaternion value;
        public Rotation rotation;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            if (rotation == null) rotation = new Rotation();
            entity.AddComponent(rotation);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(rotation);
        }

        private void Update()
        {
            value.x = rotation.X = transform.rotation.x;
            value.y = rotation.Y = transform.rotation.y;
            value.z = rotation.Z = transform.rotation.z;
            value.w = rotation.W = transform.rotation.w;
        }
    }
}