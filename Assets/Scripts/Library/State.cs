using System;
using System.Collections.Generic;

namespace EntityComponentState
{
    public abstract class State : IToBytes
    {
        public int tick;
        
        public virtual SerializableListEntity entities { get; protected set; } = new SerializableListEntity();
        public abstract IEnumerable<Type> types { get; protected set; }

        public virtual State Clone()
        {
            var state = (State)Activator.CreateInstance(GetType());
            state.tick = tick;
            state.entities = entities.Clone();
            state.types = types;
            return state;
        }

        public override string ToString()
        {
            var output = $"State [Tick: {tick}]\r\n";
            foreach (var componentType in types)
            {
                output += $"  {componentType.Name}\r\n";
                foreach (var entity in entities)
                    if (entity.HasComponent(componentType))
                        output += $"    {entity.GetComponent(componentType)}\r\n";
            }
            return output;
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            bytes.Enqueue(tick);
            bytes.Enqueue(entities);

            foreach (var componentType in types)
            {
                foreach (var entity in entities)
                {
                    var hasComponent = entity.HasComponent(componentType);
                    bytes.Enqueue(hasComponent);
                    if (hasComponent)
                        bytes.Enqueue(entity.GetComponent(componentType));
                }
            }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            tick = bytes.GetInt();
            entities.FromBytes(bytes);

            foreach (var componentType in types)
            {
                foreach (var entity in entities)
                {
                    var hasComponent = bytes.GetBool();
                    if (hasComponent)
                        entity.AddComponent(bytes.GetIToBytes<Component>(componentType));
                }
            }
        }
    }

    public abstract class CompressedState : State
    {
        public override SerializableListEntity entities { get; protected set; } = new SerializableListEntityCompressed();
    }
}
