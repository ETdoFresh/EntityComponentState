using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public class State
    {
        public int tick;
        public List<Entity> entities = new List<Entity>();

        public static State Create(int tick, List<Entity> entities)
        {
            var newState = new State
            {
                tick = tick
            };
            foreach (var entity in entities.OrderBy(e => e.id))
                newState.entities.Add(entity.Clone());
            return newState;
        }

        public State Clone()
        {
            return Create(tick, entities);
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

        public override string ToString()
        {
            var output = $"State [Tick: {tick}]\r\n";
            foreach (var componentType in Component.types)
            {
                output += $"  {componentType.Name} [Count: {GetCount(componentType)}]\r\n";
                foreach (var component in GetComponents(componentType))
                    output += $"    {component}\r\n";
            }

            return output;
        }

        public byte[] ToBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(tick.ToBytes());

            foreach (var componentType in Component.types)
            {
                bytes.AddRange(GetCount(componentType).ToBytes());
                foreach (var component in GetComponents(componentType))
                    bytes.AddRange(component.ToBytes());
            }
            return bytes.ToArray();
        }

        public byte[] ToCompressedBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(tick.ToBytes());

            foreach (var componentType in Component.types)
            {
                bytes.AddRange(((byte)GetCount(componentType)).ToBytes());
                foreach (var component in GetComponents(componentType))
                    bytes.AddRange(component.ToBytes());
            }
            return bytes.ToArray();
        }

        public string ToByteHexString()
        {
            var output = "";
            output += tick.ToByteHexString();

            foreach (var componentType in Component.types)
            {
                output += $" {BitConverter.GetBytes(GetCount(componentType)).ToHexString()}";
                foreach (var component in GetComponents(componentType))
                    output += $" {component.ToByteHexString()}";
            }
            var count = output.Replace(" ", "").Length / 2;
            return $"{output} [{count}]";
        }

        public string ToCompressedByteHexString()
        {
            var output = "";
            output += tick.ToByteHexString();

            foreach (var componentType in Component.types)
            {
                output += $" {BitConverter.GetBytes((byte)GetCount(componentType)).ToHexString()}";
                foreach (var component in GetComponents(componentType))
                    output += $" {component.ToCompressedByteHexString()}";
            }
            var count = output.Replace(" ", "").Length / 2;
            return $"{output} [{count}]";
        }
    }
}
