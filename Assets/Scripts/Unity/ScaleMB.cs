using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    public class ScaleMB : ComponentMB
    {
        public UnityVector3 value;
        public Scale scale;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            
            if (entity.HasComponent<Scale>())
                scale = entity.GetComponent<Scale>();
            else if (scale == null)
                scale = new Scale();

            if (!entity.HasComponent<Scale>())
                entity.AddComponent(scale);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(scale);
        }

        private void Update()
        {
            value.x = scale.X = transform.localScale.x;
            value.y = scale.Y = transform.localScale.y;
            value.z = scale.Z = transform.localScale.z;
        }
    }
}