using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    public class PositionMB : ComponentMB
    {
        public UnityVector3 value;
        public Position position;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;

            if (entity.HasComponent<Position>())
                position = entity.GetComponent<Position>();
            else if (position == null)
                position = new Position();

            if (!entity.HasComponent<Position>())
                entity.AddComponent(position);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(position);
        }

        private void Update()
        {
            value.x = position.X = transform.position.x;
            value.y = position.Y = transform.position.y;
            value.z = position.Z = transform.position.z;
        }
    }
}