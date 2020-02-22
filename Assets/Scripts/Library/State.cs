using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    [Serializable]
    public class State : IToBytes
    {
        public int tick;
        public SerializableList<Entity> entities;
        [NonSerialized] public List<Type> types;

        public State()
        {
            entities = new SerializableList<Entity>();
            types = new List<Type>
            {
                typeof(Position),
                typeof(Rotation),
                typeof(Scale),
                typeof(Velocity),
                typeof(AngularVelocity),
                typeof(Sprite),
                typeof(AnimationFrame),
                typeof(Primitive),
                typeof(Name)
            };
        }

        public static State Create(int tick, IEnumerable<Entity> entities)
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
            foreach (var componentType in types)
            {
                output += $"  {componentType.Name} [Count: {GetCount(componentType)}]\r\n";
                foreach (var component in GetComponents(componentType))
                    output += $"    {component}\r\n";
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
            entities.Clear();
            entities.AddRange(bytes.GetIToBytess<Entity>());

            foreach (var componentType in types)
            {
                foreach (var entity in entities)
                {
                    var hasComponent = bytes.GetBool();
                    if (hasComponent)
                        entity.AddComponent(bytes.GetIToBytes<Component>());
                }
            }
        }
    }

    public class CompressedState : State
    {
        public CompressedState()
        {
            entities = new SerializableListByteCount<Entity>();
            types = new List<Type>
            {
                typeof(CompressedPosition),
                typeof(CompressedRotation),
                typeof(CompressedScale),
                typeof(CompressedVelocity),
                typeof(CompressedAngularVelocity),
                typeof(Sprite),
                typeof(AnimationFrame),
                typeof(Primitive),
                typeof(Name)
            };
        }
    }
}
