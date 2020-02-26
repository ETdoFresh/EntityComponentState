using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public abstract class DeltaState : IToBytes
    {
        public int startStateTick;
        public int endStateTick;
        public virtual SerializableListEntity spawns { get; protected set; } = new SerializableListEntity();
        public virtual SerializableListEntity despawns { get; protected set; } = new SerializableListEntity();

        public abstract IEnumerable<Type> componentTypes { get; }
        public abstract Type stateType { get; }
        private List<Change> changes = new List<Change>();

        public DeltaState() { }

        public DeltaState(State startState, State endState)
        {
            Create(startState, endState);
        }

        public void Create(State startState, State endState)
        {
            Clear();
            startStateTick = startState.tick;
            endStateTick = endState.tick;

            var startEntities = startState.entities;
            var endEntities = endState.entities;

            spawns.AddRange(endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id)));
            despawns.AddRange(startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id)));

            foreach (var endEntity in endState.entities)
                foreach (var componentType in endState.componentTypes)
                {
                    var change = new Change { componentType = componentType, entityId = endEntity.id, delta = null };
                    changes.Add(change);

                    var endComponent = endEntity.GetComponent(componentType);
                    if (endComponent != null)
                    {
                        var startEntity = startState.entities.Where(entity => entity.id == endEntity.id).FirstOrDefault();
                        if (startEntity == null)
                            change.delta = endComponent;
                        else
                        {
                            var startComponent = startEntity.GetComponent(componentType);
                            if (startComponent != endComponent)
                                change.delta = endComponent;
                        }
                    }
                }
        }

        public void Clear()
        {
            startStateTick = 0;
            endStateTick = 0;
            spawns.Clear();
            despawns.Clear();
            changes.Clear();
        }

        public virtual State GenerateEndState(State startState)
        {
            if (startState.tick != startStateTick)
                throw new ArgumentException("Start State Tick did not match Delta State Start Tick. Are you sure you wanted to use this?");

            var endState = (State)Activator.CreateInstance(startState.GetType());
            endState.tick = endStateTick;
            endState.entities.AddRange(startState.entities.Union(spawns).Except(despawns));
            return endState;
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startStateTick} End Tick: {endStateTick}]\r\n";

            output += $"\r\nSpawns [Count: {spawns.Count()}]\r\n";
            foreach (var entity in spawns)
                output += $"{entity.id} ";

            output += $"\r\nDespawns [Count: {despawns.Count()}]\r\n";
            foreach (var entity in despawns)
                output += $"{entity.id} ";

            output += "\r\n";

            foreach (var componentType in componentTypes)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);
                output += $"  {componentType.Name} [Count: {componentChanges.Count()}]\r\n";
                foreach (var component in componentChanges.Select(change => change.delta))
                    if (component is null)
                        output += $"    SKIP\r\n";
                    else
                        output += $"    {component}\r\n";
            }

            return output;
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            bytes.Enqueue(startStateTick);
            bytes.Enqueue(endStateTick);
            bytes.Enqueue(spawns);
            bytes.Enqueue(despawns);
            foreach (var componentType in componentTypes)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType).OrderBy(change => change.entityId);

                var i = 0;
                var skip = 0;
                while (i < componentChanges.Count())
                {
                    var delta = componentChanges.ElementAt(i).delta;
                    if (delta is null)
                    {
                        i++;
                        skip++;
                    }
                    else
                    {
                        bytes.Enqueue(skip);
                        bytes.Enqueue(delta);
                        i++;
                        skip = 0;
                    }
                }
                if (skip > 0)
                    bytes.Enqueue(skip);
            }
            return bytes;
        }

        public void FromBytes(ByteQueue bytes)
        {
            FromBytes(bytes, null);
        }

        public void FromBytes(ByteQueue bytes, State storedStartState)
        {
            changes.Clear();
            startStateTick = bytes.GetInt();
            endStateTick = bytes.GetInt();
            spawns = bytes.GetIToBytes<SerializableListEntity>(spawns.GetType());
            despawns = bytes.GetIToBytes<SerializableListEntity>(despawns.GetType());

            if (storedStartState == null)
                return; //throw new ArgumentException("If not start state passed, delta state cannot reconstruct end state");

            if (storedStartState.tick != startStateTick)
                return; //throw new ArgumentException($"Cannot build tick {endState.tick} from tick {storedStartState.tick}. Expecting tick {startState.tick}.");

            var entities = storedStartState.entities.Union(spawns);
            foreach (var entity in entities)
                foreach (var componentType in componentTypes)
                    changes.Add(new Change { componentType = componentType, entityId = entity.id });

            foreach (var componentType in componentTypes)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);

                var i = 0;
                while (i < componentChanges.Count())
                {
                    var skip = bytes.GetInt();
                    if (i + skip >= componentChanges.Count())
                        break;

                    i += skip;
                    var componentChange = componentChanges.ElementAt(i);
                    componentChange.delta = bytes.GetIToBytes<Component>(componentType);
                    var entity = entities.Where(endEntity => endEntity.id == componentChange.entityId).First();
                    entity.AddComponent(componentChange.delta);
                }
            }
        }

        public static DeltaState operator +(DeltaState lhs, DeltaState rhs)
        {
            if (lhs.endStateTick != rhs.startStateTick)
                throw new ArgumentException("Cannot combine Delta States where endStateTick of Left Hand Side does not equal startStateTick of Right Hand Side");

            var newDeltaState = (DeltaState)Activator.CreateInstance(lhs.GetType());
            newDeltaState.startStateTick = lhs.startStateTick;
            newDeltaState.endStateTick = rhs.endStateTick;
            newDeltaState.spawns.AddRange(lhs.spawns.Union(rhs.spawns));
            newDeltaState.despawns.AddRange(lhs.despawns.Union(rhs.spawns));
            foreach (var rhsChange in rhs.changes)
                if (rhsChange.delta == null)
                    rhsChange.delta = lhs.changes
                        .Where(lhsChange => lhsChange.entityId == rhsChange.entityId && lhsChange.componentType == rhsChange.componentType)
                        .Select(lhsChange => lhsChange.delta)
                        .FirstOrDefault();
            return newDeltaState;
        }

        private class Change
        {
            public int entityId;
            public Type componentType;
            public Component delta;

            public override string ToString()
            {
                if (delta != null)
                    return $"{entityId} {componentType.Name} {delta}";
                else
                    return $"SKIP! {entityId} {componentType.Name}";
            }
        }
    }
}
