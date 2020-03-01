using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public abstract class State : IToBytes
    {
        public int tick;

        public virtual SerializableListEntity entities { get; } = new SerializableListEntity();
        public abstract IEnumerable<Type> componentTypes { get; }

        public virtual void Clear()
        {
            tick = 0;
            entities.Clear();
        }

        public virtual State Clone()
        {
            var state = (State)Activator.CreateInstance(GetType());
            state.tick = tick;
            state.entities.AddRange(entities.Clone());
            return state;
        }

        public override string ToString()
        {
            var output = $"State [Tick: {tick}]\r\n";
            foreach (var componentType in componentTypes)
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

            foreach (var componentType in componentTypes)
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
            entities.FromBytes(bytes);

            foreach (var componentType in componentTypes)
            {
                foreach (var entity in entities)
                {
                    var hasComponent = bytes.GetBool();
                    if (hasComponent)
                        entity.AddComponent(bytes.GetIToBytes<Component>(componentType));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                if (tick != other.tick) return false;
                if (entities.Count != other.entities.Count) return false;

                foreach (var entity in entities)
                    if (!other.entities.Contains(entity))
                        return false;

                foreach (var entity in entities)
                    foreach (var component in entity.components)
                        if (!other.entities.First(e => e == entity).components.Contains(component))
                            return false;

                return true;
            }
            return false;
        }

        public static bool operator ==(State lhs, State rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(State lhs, State rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return tick.GetHashCode();
        }

        public static State Lerp(State startState, State endState, float t)
        {
            var newState = startState.Clone();
            var startStateEntities = newState.entities.Union(endState.entities);
            var endStateEntities = endState.entities.Union(startState.entities);
            foreach (var newStateEntity in newState.entities)
            {
                var endStateEntity = endStateEntities.Where(e => e == newStateEntity).FirstOrDefault();
                if (endStateEntity != null)
                {
                    var startStateEntity = startStateEntities.Where(e => e == newStateEntity).FirstOrDefault();
                    foreach (var newStateComponent in newStateEntity.components)
                    {
                        var type = newStateComponent.GetType();
                        var startStateComponent = startStateEntity.GetComponent(type);
                        var endStateComponent = endStateEntity.GetComponent(type);
                        if (endStateComponent != null)
                            newStateComponent.CopyValuesFrom(Component.Lerp(startStateComponent, endStateComponent, t));
                    }
                }
            }
            return newState;
        }
    }

    public abstract class CompressedState : State
    {
        public override SerializableListEntity entities { get; } = new SerializableListEntityCompressed();
    }
}
