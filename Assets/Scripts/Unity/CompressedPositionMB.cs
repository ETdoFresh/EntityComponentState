using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    public class CompressedPositionMB : ComponentMB<CompressedPosition>
    {
        public UnityVector3 value;

        private void Update()
        {
            value.x = component.X = transform.position.x;
            value.y = component.Y = transform.position.y;
            value.z = component.Z = transform.position.z;
        }
    }
}