using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public abstract class DeltaState : IToBytes
    {
        public virtual State startState { get; protected set; }
        public virtual State endState { get; protected set; }
        public virtual SerializableListEntity spawns { get; protected set; } = new SerializableListEntity();
        public virtual SerializableListEntity despawns { get; protected set; } = new SerializableListEntity();
        private List<Change> changes = new List<Change>();

        public DeltaState() { }

        public DeltaState(State startState, State endState)
        {
            Create(startState, endState);
        }

        public void Create(State startState, State endState)
        {
            Clear();
            this.startState.tick = startState.tick;
            this.endState.tick = endState.tick;
            this.startState.entities.AddRange(startState.entities);
            this.endState.entities.AddRange(endState.entities);
            var startEntities = startState.entities;
            var endEntities = endState.entities;

            spawns.AddRange(endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id)));
            despawns.AddRange(startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id)));

            foreach (var endEntity in endState.entities)
                foreach (var componentType in endState.types)
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
            startState.Clear();
            endState.Clear();
            spawns.Clear();
            despawns.Clear();
            changes.Clear();
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startState.tick} End Tick: {endState.tick}]\r\n";

            output += $"\r\nSpawns [Count: {spawns.Count()}]\r\n";
            foreach (var entity in spawns)
                output += $"{entity.id} ";

            output += $"\r\nDespawns [Count: {despawns.Count()}]\r\n";
            foreach (var entity in despawns)
                output += $"{entity.id} ";

            output += "\r\n";

            foreach (var componentType in endState.types)
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
            bytes.Enqueue(startState.tick);
            bytes.Enqueue(endState.tick);
            bytes.Enqueue(spawns);
            bytes.Enqueue(despawns);
            foreach (var componentType in endState.types)
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
            startState.Clear();
            endState.Clear();
            changes.Clear();

            startState.tick = bytes.GetInt();
            endState.tick = bytes.GetInt();
            spawns = bytes.GetIToBytes<SerializableListEntity>(spawns.GetType());
            despawns = bytes.GetIToBytes<SerializableListEntity>(despawns.GetType());

            if (storedStartState == null)
                return; //throw new ArgumentException("If not start state passed, delta state cannot reconstruct end state");

            if (storedStartState.tick != startState.tick)
                return; //throw new ArgumentException($"Cannot build tick {endState.tick} from tick {storedStartState.tick}. Expecting tick {startState.tick}.");

            startState.entities.AddRange(storedStartState.entities.Clone());
            endState.entities.AddRange(storedStartState.entities.Union(spawns).Except(despawns));

            foreach (var entity in endState.entities)
                foreach (var componentType in endState.types)
                    changes.Add(new Change { componentType = componentType, entityId = entity.id });

            foreach (var componentType in endState.types)
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
                    var entity = endState.entities.Where(endEntity => endEntity.id == componentChange.entityId).First();
                    entity.AddComponent(componentChange.delta);
                }
            }
        }

        private class Change
        {
            public int entityId;
            public Type componentType;
            public Component delta;

            public override string ToString()
            {
                return $"{entityId} {componentType.Name} {delta}";
            }
        }
    }
}
