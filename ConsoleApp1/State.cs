using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class State
    {
        public static List<Type> componentTypes = new List<Type>
        {
            typeof(Position), typeof(Rotation), typeof(Scale),
            typeof(Velocity), typeof(AngularVelocity),
            typeof(Sprite), typeof(AnimationFrame)
        };

        public int tick;
        public List<Entity> entities = new List<Entity>();

        public static State Create(int tick, List<Entity> entities)
        {
            var newState = new State();
            newState.tick = tick;
            foreach (var entity in entities.OrderBy(e => e.id))
                newState.entities.Add(entity.Clone());
            return newState;
        }

        private int GetCount(Type type)
        {
            var count = 0;
            foreach (var entity in entities)
                if (entity.HasComponent(type))
                    count++;
            return count;
        }

        private IEnumerable<Component> GetComponents(Type type)
        {
            foreach (var entity in entities)
                if (entity.HasComponent(type))
                    yield return entity.GetComponent(type);
        }

        private int GetCount<T>() where T : Component
        {
            var count = 0;
            foreach (var entity in entities)
                if (entity.HasComponent<T>())
                    count++;
            return count;
        }

        private IEnumerable<T> GetComponents<T>() where T : Component
        {
            foreach (var entity in entities)
                if (entity.HasComponent<T>())
                    yield return entity.GetComponent<T>();
        }

        public override string ToString()
        {
            var output = $"State [Tick: {tick}]\r\n";
            foreach (var componentType in componentTypes)
            {
                output += $"  {componentType.Name} [Count: {GetCount(componentType)}]\r\n";
                foreach (var component in GetComponents(componentType))
                    output += $"    {component}\r\n";
            }

            return output;
        }

        public string ToByteHexString()
        {
            var output = "";
            output += tick.ToByteHexString();

            foreach (var componentType in componentTypes)
            {
                output += $" {BitConverter.GetBytes(GetCount(componentType)).ToHexString()}";
                foreach (var component in GetComponents(componentType))
                    output += $" {component.ToByteHexString()}";
            }
            var count = output.Replace(" ", "").Length / 2;
            return $"{output} [{count}]";
        }
    }
}
