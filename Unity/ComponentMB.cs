using UnityEngine;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(EntityMB))]
    public abstract class ComponentMB : MonoBehaviour 
    {
        public Entity entity;
    }
}