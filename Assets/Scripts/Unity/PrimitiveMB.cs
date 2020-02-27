using UnityEngine;
using UnityPrimitiveType = UnityEngine.PrimitiveType;
using PrimitiveType = EntityComponentState.Primitive.PrimitiveType;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PrimitiveMB : ComponentMB<Primitive>
    {
        public UnityPrimitiveType value;
        private Mesh mesh;
        public MeshFilter meshFilter;

        protected override void OnEnable()
        {
            base.OnEnable();
            meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            // Saves on garbage collection
            if (mesh == meshFilter.mesh)
                return;

            var meshName = meshFilter.mesh.name;
            if (meshName.StartsWith("Sphere"))
                component.primitiveType = PrimitiveType.Sphere;
            else if (meshName.StartsWith("Capsule"))
                component.primitiveType = PrimitiveType.Capsule;
            else if (meshName.StartsWith("Cylinder"))
                component.primitiveType = PrimitiveType.Cylinder;
            else if (meshName.StartsWith("Cube"))
                component.primitiveType = PrimitiveType.Cube;
            else if (meshName.StartsWith("Plane"))
                component.primitiveType = PrimitiveType.Plane;
            else if (meshName.StartsWith("Quad"))
                component.primitiveType = PrimitiveType.Quad;

            value = (UnityPrimitiveType)(int)component.primitiveType;
            mesh = meshFilter.mesh;
        }
    }
}