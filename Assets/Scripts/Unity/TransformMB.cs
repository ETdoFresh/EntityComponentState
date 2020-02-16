using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(EntityMB))]
    public class TransformMB : MonoBehaviour
    {
        public UnityVector3 position;
        public Quaternion rotation;
        public UnityVector3 scale;

        public Entity entity;
        public Position positionComponent;
        public Rotation rotationComponent;
        public Scale scaleComponent;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            if (positionComponent == null) positionComponent = new Position();
            if (rotationComponent == null) rotationComponent = new Rotation();
            if (scaleComponent == null) scaleComponent = new Scale();
            entity.AddComponent(positionComponent, rotationComponent, scaleComponent);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(positionComponent, rotationComponent, scaleComponent);
        }

        private void Update()
        {
            position.x = positionComponent.X = transform.position.x;
            position.y = positionComponent.Y = transform.position.y;
            position.z = positionComponent.Z = transform.position.z;
            rotation.x = rotationComponent.X = transform.rotation.x;
            rotation.y = rotationComponent.Y = transform.rotation.y;
            rotation.z = rotationComponent.Z = transform.rotation.z;
            rotation.w = rotationComponent.W = transform.rotation.w;
            scale.x = scaleComponent.X = transform.localScale.x;
            scale.y = scaleComponent.Y = transform.localScale.y;
            scale.z = scaleComponent.Z = transform.localScale.z;
        }
    }
}