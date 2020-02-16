using System;
using System.Collections.Generic;
using System.Linq;
using Component = EntityComponentState.Component;

namespace EntityComponentState
{
    public class Entity
    {
        public static readonly Entity NULL = new Entity(-1);

        public int id;
        public List<Component> components = new List<Component>();

        public Entity()
        {
            id = IDAssignment.GetID(GetType().FullName);
        }

        public Entity(int id)
        {
            this.id = id;
        }

        public Entity Clone()
        {
            var newEntityData = new Entity(this.id);
            foreach (var component in components)
                newEntityData.AddComponent(component.Clone());
            return newEntityData;
        }

        public void AddComponent(params Component[] components)
        {
            if (components == null)
                throw new ArgumentNullException("Cannot Entity.AddComponents(null)");

            foreach (var component in components)
                if (component == null)
                    throw new ArgumentNullException("Cannot Entity.AddComponents(...,null,...)");

            this.components.AddRange(components);
            foreach (var component in components)
                component.entity = this;
        }

        public void RemoveComponent(params Component[] components)
        {
            foreach (var component in components)
            {
                this.components.Remove(component);
                component.entity = Entity.NULL;
            }
        }

        public void Destroy()
        {
            IDAssignment.ReleaseID(id);
        }

        public bool HasComponent<T>() where T : Component
        {
            return components.Any(c => c is T);
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)components.Where(c => c is T).FirstOrDefault();
        }

        public bool HasComponent(Type type)
        {
            return components.Any(c => type.IsAssignableFrom(c.GetType()));
        }

        public Component GetComponent(Type type)
        {
            return components.Where(c => type.IsAssignableFrom(c.GetType())).FirstOrDefault();
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is Entity other)
            {
                if (this.id != other.id) return false;
                foreach (var component in components)
                    if (!other.HasComponent(component.GetType()))
                        return false;
                    else if (other.GetComponent(component.GetType()) != component)
                        return false;
                return true;
            }
            return false;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Entity lhs, Entity rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
