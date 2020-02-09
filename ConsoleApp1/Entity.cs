using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Entity
    {
        public static Entity NULL = new Entity(-1);

        public int id;
        public List<Component> components = new List<Component>();

        public Entity()
        {
            id = IDAssignment.GetID(GetType().FullName);
        }

        private Entity(int id)
        {
            this.id = id;
        }

        public Entity Clone()
        {
            var newEntityData = new Entity(this.id);
            foreach (var component in components)
                newEntityData.AddComponents(component.Clone());
            return newEntityData;
        }

        public void AddComponents(params Component[] components)
        {
            foreach (var component in components)
                if (component == null)
                    throw new ArgumentNullException("Cannot AddComponent Null");

            this.components.AddRange(components);
            foreach (var component in components)
                component.entity = this;
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
            return components.Where(c => c is T).FirstOrDefault() as T;
        }
    }
}
