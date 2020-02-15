using UnityEngine;

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

        public static void Apply(this Component component, GameObject gameObject)
        {
            if (component is Position position)
            {
                gameObject.transform.position = position.value.ToUnityVector3();
                return;
            }
            if (component is Rotation rotation)
            {
                gameObject.transform.rotation = rotation.value.ToUnityQuaternion();
                return;
            }
            if (component is Scale scale)
            {
                gameObject.transform.localScale = scale.value.ToUnityVector3();
                return;
            }
            if (component is Velocity velocity)
            {
                var rigidbody = gameObject.GetComponent<Rigidbody>();
                if (!rigidbody) return;
                rigidbody.velocity = velocity.value.ToUnityVector3();
                return;
            }
            if (component is AngularVelocity angularVelocity)
            {
                var rigidbody = gameObject.GetComponent<Rigidbody>();
                if (!rigidbody) return;
                rigidbody.angularVelocity = angularVelocity.value.ToUnityVector3();
                return;
            }
            if (component is Name name)
            {
                gameObject.name = name.name;
                return;
            }
            //if (component is Primitive primitive)
            //{
            //}
        }
    }
}