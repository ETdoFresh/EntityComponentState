using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public class State : IToBytes
    {
        public int tick;
        public SerializableList<Entity> entities;
        [NonSerialized] public IEnumerable<Type> types;

        public State()
        {
            if (types == null)
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
            
            if (entities == null)
                entities = new SerializableList<Entity>();
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

    public class CompressedState : State
    {
        public CompressedState()
        {
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
            entities = new SerializableListByteCount<Entity>();
        }
    }
}
