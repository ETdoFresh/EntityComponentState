using UnityEngine;

namespace EntityComponentState.Unity
{
    public static class MeshHelper
    {
        private static Mesh capsule;
        private static Mesh cube;
        private static Mesh cylinder;
        private static Mesh plane;
        private static Mesh quad;
        private static Mesh sphere;

        public static Mesh Capsule => capsule != null ? capsule : capsule = GetMesh(PrimitiveType.Capsule);
        public static Mesh Cube => cube != null ? cube : cube = GetMesh(PrimitiveType.Cube);
        public static Mesh Cylinder => cylinder != null ? cylinder : cylinder = GetMesh(PrimitiveType.Cylinder);
        public static Mesh Plane => plane != null ? plane : plane = GetMesh(PrimitiveType.Plane);
        public static Mesh Quad => quad != null ? quad : quad = GetMesh(PrimitiveType.Quad);
        public static Mesh Sphere => sphere != null ? sphere : sphere = GetMesh(PrimitiveType.Sphere);

        public static Mesh GetMesh(PrimitiveType primitiveType)
        {
            var gameObject = GameObject.CreatePrimitive(primitiveType);
            var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            Object.DestroyImmediate(gameObject);
            return mesh;
        }
    }
}