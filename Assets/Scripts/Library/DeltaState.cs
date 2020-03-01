using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public abstract class DeltaState : IToBytes
    {
        public int startStateTick;
        public int endStateTick;
        public virtual LongValue entityCount { get; } = new IntValue();
        public virtual SerializableListEntity spawns { get; } = new SerializableListEntity();
        public virtual SerializableListEntity despawns { get; } = new SerializableListEntity();

        public abstract IEnumerable<Type> componentTypes { get; }
        public abstract Type stateType { get; }
        private Dictionary<Type, List<Component>> changes = new Dictionary<Type, List<Component>>();

        public DeltaState()
        {
            foreach (var componentType in componentTypes)
                changes.Add(componentType, new List<Component>());
        }

        public DeltaState(State startState, State endState)
        {
            foreach (var componentType in componentTypes)
                changes.Add(componentType, new List<Component>());
            Create(startState, endState);
        }

        public void Create(State startState, State endState)
        {
            Clear();
            startStateTick = startState.tick;
            endStateTick = endState.tick;
            entityCount.value = endState.entities.Count;

            var startEntities = startState.entities.Clone();
            var endEntities = endState.entities.Clone();

            spawns.AddRange(endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id)));
            despawns.AddRange(startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id)));

            foreach (var componentType in endState.componentTypes)
                foreach (var endEntity in endState.entities)
                {
                    Component change = null;
                    var endComponent = endEntity.GetComponent(componentType);
                    if (endComponent != null)
                    {
                        var startEntity = startState.entities.Where(entity => entity.id == endEntity.id).FirstOrDefault();
                        if (startEntity == null)
                            change = endComponent;
                        else
                        {
                            var startComponent = startEntity.GetComponent(componentType);
                            if (startComponent != endComponent)
                                change = endComponent;
                        }
                    }
                    changes[componentType].Add(change);
                }
        }

        public virtual DeltaState Clone()
        {
            var deltaState = (DeltaState)Activator.CreateInstance(GetType());
            deltaState.startStateTick = startStateTick;
            deltaState.endStateTick = endStateTick;
            deltaState.entityCount.value = entityCount.value;
            deltaState.spawns.AddRange(spawns);
            deltaState.despawns.AddRange(despawns);
            foreach (var componentType in componentTypes)
                deltaState.changes[componentType].AddRange(changes[componentType]);
            return deltaState;
        }

        public void Clear()
        {
            startStateTick = 0;
            endStateTick = 0;
            entityCount.value = 0;
            spawns.Clear();
            despawns.Clear();
            foreach (var componentType in componentTypes)
                changes[componentType].Clear();
        }

        public virtual State GenerateEndState(State startState)
        {
            if (startState.tick != startStateTick)
                throw new ArgumentException("Start State Tick did not match Delta State Start Tick. Are you sure you wanted to use this?");

            var endState = (State)Activator.CreateInstance(startState.GetType());
            endState.tick = endStateTick;
            endState.entities.AddRange(startState.entities.Clone().Union(spawns).Except(despawns).OrderBy(e => e.id));
            foreach (var componentType in componentTypes)
                for (var i = 0; i < entityCount.value; i++)
                    if (changes[componentType][i] != null)
                        if (endState.entities[i].HasComponent(componentType))
                            endState.entities[i].GetComponent(componentType).CopyValuesFrom(changes[componentType][i]);
                        else
                            endState.entities[i].AddComponent(changes[componentType][i]);
            return endState;
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startStateTick} End Tick: {endStateTick}] Entity Count: {entityCount}\r\n";

            output += $"\r\nSpawns [Count: {spawns.Count()}]\r\n";
            foreach (var entity in spawns)
                output += $"{entity.id} ";

            output += $"\r\nDespawns [Count: {despawns.Count()}]\r\n";
            foreach (var entity in despawns)
                output += $"{entity.id} ";

            output += "\r\n";

            foreach (var componentType in componentTypes)
            {
                output += $"  ----{componentType.Name}----\r\n";
                foreach (var change in changes[componentType])
                    if (change is null)
                        output += $"    SKIP\r\n";
                    else
                        output += $"    {change}\r\n";
            }

            return output;
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            bytes.Enqueue(startStateTick);
            bytes.Enqueue(endStateTick);
            bytes.Enqueue(entityCount);
            bytes.Enqueue(spawns);
            bytes.Enqueue(despawns);
            foreach (var componentType in componentTypes)
                foreach (var change in changes[componentType])
                {
                    var hasChanges = change != null;
                    bytes.Enqueue(hasChanges);
                    if (hasChanges)
                        bytes.Enqueue(change);
                }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            Clear();
            startStateTick = bytes.GetInt();
            endStateTick = bytes.GetInt();
            entityCount.value = bytes.GetIToBytes<LongValue>(entityCount.GetType()).value;
            spawns.AddRange(bytes.GetIToBytes<SerializableListEntity>(spawns.GetType()));
            despawns.AddRange(bytes.GetIToBytes<SerializableListEntity>(despawns.GetType()));

            foreach (var componentType in componentTypes)
                for (int i = 0; i < entityCount.value; i++)
                {
                    var hasChanges = bytes.GetBool();
                    if (hasChanges)
                        changes[componentType].Add(bytes.GetIToBytes<Component>(componentType));
                    else
                        changes[componentType].Add(null);
                }
        }
    }

    public abstract class CompressedDeltaState : DeltaState
    {
        public override LongValue entityCount { get; } = new ByteValue();
        public override SerializableListEntity spawns { get; } = new SerializableListEntityCompressed();
        public override SerializableListEntity despawns { get; } = new SerializableListEntityCompressed();

        public CompressedDeltaState() { }
        public CompressedDeltaState(State startState, State endState) : base(startState, endState) { }
    }
}