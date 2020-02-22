using System;
using UnityEngine;

namespace EntityComponentState.Unity
{
    [RequireComponent(typeof(EntityMB))]
    public abstract class ComponentMB<T> : MonoBehaviour where T : Component
    {
        public Entity entity;
        public T component;

        protected virtual void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;

            if (entity.HasComponent<T>())
                component = entity.GetComponent<T>();
            else if (component == null)
                component = Activator.CreateInstance<T>();

            if (!entity.HasComponent<T>())
                entity.AddComponent(component);
        }

        protected virtual void OnDisable()
        {
            entity.RemoveComponent(component);
        }
    }
}