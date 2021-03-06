﻿using UnityEngine;

namespace EntityComponentState.Unity
{
    public static class ComponentExtension
    {
        public static UnityEngine.Vector3 ToUnityVector3(this Vector3 vector3)
        {
            return new UnityEngine.Vector3(vector3.x, vector3.y, vector3.z);
        }

        public static Quaternion ToUnityQuaternion(this Vector4 vector4)
        {
            return new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        public static PrimitiveType ToUnityPrimitiveType(this Primitive.PrimitiveType primitiveType)
        {
            if (primitiveType == Primitive.PrimitiveType.Capsule) return PrimitiveType.Capsule;
            if (primitiveType == Primitive.PrimitiveType.Cube) return PrimitiveType.Cube;
            if (primitiveType == Primitive.PrimitiveType.Cylinder) return PrimitiveType.Cylinder;
            if (primitiveType == Primitive.PrimitiveType.Plane) return PrimitiveType.Plane;
            if (primitiveType == Primitive.PrimitiveType.Quad) return PrimitiveType.Quad;
            if (primitiveType == Primitive.PrimitiveType.Sphere) return PrimitiveType.Sphere;
            throw new System.ArgumentException();
        }

        public static void Apply(this Component component, GameObject gameObject)
        {
            if (component is Position position)
            {
                gameObject.transform.position = position.value.ToUnityVector3();
                return;
            }
            else if (component is Rotation rotation)
            {
                gameObject.transform.rotation = rotation.value.ToUnityQuaternion();
                return;
            }
            else if (component is Scale scale)
            {
                gameObject.transform.localScale = scale.value.ToUnityVector3();
                return;
            }
            else if (component is CompressedPosition compressedPosition)
            {
                gameObject.transform.position = compressedPosition.value.ToUnityVector3();
                return;
            }
            else if (component is CompressedRotation compressedRotation)
            {
                gameObject.transform.rotation = compressedRotation.value.ToUnityQuaternion();
                return;
            }
            else if (component is CompressedScale compressedScale)
            {
                gameObject.transform.localScale = compressedScale.value.ToUnityVector3();
                return;
            }
            else if (component is Velocity velocity)
            {
                var rigidbody = gameObject.GetComponent<Rigidbody>();
                if (!rigidbody) return;
                rigidbody.velocity = velocity.value.ToUnityVector3();
                return;
            }
            else if (component is AngularVelocity angularVelocity)
            {
                var rigidbody = gameObject.GetComponent<Rigidbody>();
                if (!rigidbody) return;
                rigidbody.angularVelocity = angularVelocity.value.ToUnityVector3();
                return;
            }
            else if (component is Name name)
            {
                gameObject.name = name.name;
                return;
            }
            else if (component is Primitive primitive)
            {
                var meshFilter = gameObject.GetComponent<MeshFilter>();
                if (!meshFilter) return;
                meshFilter.sharedMesh = MeshHelper.GetMesh(primitive.primitiveType.ToUnityPrimitiveType());
            }
        }
    }
}