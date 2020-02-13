using UnityEngine;
using UnityPrimitiveType = UnityEngine.PrimitiveType;
using PrimitiveType = EntityComponentState.Primitive.PrimitiveType;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PrimitiveMB : ComponentMB
    {
        public UnityPrimitiveType value;
        public Primitive primitive;
        public MeshFilter meshFilter;

        private void OnEnable()
        {
            meshFilter = GetComponent<MeshFilter>();
            entity = GetComponent<EntityMB>().entity;
            if (primitive == null) primitive = new Primitive();
            entity.AddComponent(primitive);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(primitive);
        }

        private void Update()
        {
            var meshName = meshFilter.mesh.name;
            if (meshName.StartsWith("Sphere"))
                primitive.primitiveType = PrimitiveType.Sphere;
            else if (meshName.StartsWith("Capsule"))
                primitive.primitiveType = PrimitiveType.Capsule;
            else if (meshName.StartsWith("Cylinder"))
                primitive.primitiveType = PrimitiveType.Cylinder;
            else if (meshName.StartsWith("Cube"))
                primitive.primitiveType = PrimitiveType.Cube;
            else if (meshName.StartsWith("Plane"))
                primitive.primitiveType = PrimitiveType.Plane;
            else if (meshName.StartsWith("Quad"))
                primitive.primitiveType = PrimitiveType.Quad;

            value = (UnityPrimitiveType)(int)primitive.primitiveType;
        }
    }
}