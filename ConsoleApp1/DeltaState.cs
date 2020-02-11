using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    // TODO:
    // - Delta State seems too big still. I think I need to change the format...
    // - Have startState and endState instead of startTick and endTick
    // - When creating new DeltaState from bytes, also pass in startState (endState will be created)
    // - Old format: startTick endTick entityCount entityIdList spawnCount spawnIdList despawnCount despawnIdList (Skips ComponentData)LoopEntities
    // - New format: startTick endTick spawnCount spawnIdList despawnCount despawnIdList (Skips ComponentData)LoopStartStateEntitiesAfterSpawnDespawn
    // - Think about float compression from 4 bytes to 1 bytes, maybe 10 bits? etc etc

    public class DeltaState
    {
        public int startTick;
        public int endTick;
        public IEnumerable<Entity> entities;
        public List<Entity> spawns = new List<Entity>();
        public List<Change> changes = new List<Change>();
        public List<Entity> despawns = new List<Entity>();

        public DeltaState(State startState, State endState)
        {
            this.startTick = startState.tick;
            this.endTick = endState.tick;
            var startEntities = startState.entities;
            var endEntities = endState.entities;

            var addedEntities = endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id));
            var removedEntities = startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id));

            entities = startEntities.Union(addedEntities);
            spawns.AddRange(addedEntities);
            despawns.AddRange(removedEntities);
            AddUpdates(startEntities, endEntities);
        }

        public DeltaState(byte[] bytes)
        {
            var byteQueue = new ByteQueue(bytes);
            startTick = byteQueue.GetInt32();
            endTick = byteQueue.GetInt32();
            var entityCount = byteQueue.GetInt32();
            var entities = new List<Entity>();
            this.entities = entities;
            for (int i = 0; i < entityCount; i++)
                entities.Add(new Entity(byteQueue.GetInt32()));

            var spawnCount = byteQueue.GetInt32();
            for (int i = 0; i < spawnCount; i++)
                spawns.Add(entities.Where(entity => entity.id == byteQueue.GetInt32()).First());

            var despawnCount = byteQueue.GetInt32();
            for (int i = 0; i < despawnCount; i++)
                despawns.Add(entities.Where(entity => entity.id == byteQueue.GetInt32()).First());

            foreach (var componentType in State.componentTypes)
            {
                var currentIndex = 0;
                while (currentIndex < entityCount)
                {
                    var skip = byteQueue.GetInt32();
                    for (int i = 0; i < skip; i++)
                        changes.Add(new Change { componentType = componentType, entityId = entities[currentIndex + i].id });
                    currentIndex += skip;
                    if (currentIndex >= entityCount) break;

                    var entity = entities[currentIndex];
                    var component = (Component)Activator.CreateInstance(componentType);
                    entity.AddComponents(component);
                    component.Deserialize(byteQueue);
                    changes.Add(new Change { componentType = componentType, entityId = entity.id, delta = component });
                    currentIndex++;
                }
            }
        }

        public State Apply(State startState)
        {
            var state = startState.Clone();
            state.tick = endTick;
            state.entities.AddRange(spawns);
            foreach (var componentType in State.componentTypes)
                foreach (var entity in state.entities)
                {
                    var change = changes.Where(change => change.entityId == entity.id && change.componentType == componentType).First();
                    if (change.delta != null)
                        entity.GetComponent(componentType).CopyValuesFrom(change.delta);
                }
            state.entities.RemoveAll(entity => despawns.Any(despawn => despawn.id == entity.id));
            return state;
        }

        private void AddUpdates(List<Entity> startEntities, List<Entity> endEntities)
        {
            foreach (var componentType in State.componentTypes)
                foreach (var entity in entities)
                {
                    var change = new Change { componentType = componentType, entityId = entity.id };
                    if (!startEntities.Contains(entity))
                    {
                        if (entity.HasComponent(componentType))
                        {
                            // Start State Empty --> End State Has Values == Change!
                            change.delta = entity.GetComponent(componentType);
                        }
                        // Start State Empty --> End State Empty == No Change!  (Skip)
                    }
                    else if (endEntities.Any(endEntity => endEntity.id == entity.id))
                    {
                        var endEntity = endEntities.Where(endEntity => endEntity.id == entity.id).FirstOrDefault();
                        if (entity.GetComponent(componentType) != endEntity.GetComponent(componentType))
                        {
                            // Start State Has Values --> End State Has Different Values == Change!
                            change.delta = endEntity.GetComponent(componentType);
                        }
                        // Start State Has Values --> End State Has Same Values == No Change! (Skip)
                    }
                    // Start State Has Value --> End State Empty == Iffy Skip?
                    // Otherwise Skip
                    changes.Add(change);
                }
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startTick} End Tick: {endTick}]\r\n";

            output += $"Entities [Count: {entities.Count()}]\r\n";
            foreach (var entity in entities)
                output += $"{entity.id} ";

            output += $"\r\nSpawns [Count: {spawns.Count()}]\r\n";
            foreach (var entity in spawns)
                output += $"{entity.id} ";

            output += $"\r\nDespawns [Count: {despawns.Count()}]\r\n";
            foreach (var entity in despawns)
                output += $"{entity.id} ";

            output += "\r\n";

            foreach (var componentType in State.componentTypes)
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

        public string ToByteHexString()
        {
            var output = "";
            output += startTick.ToByteHexString();
            output += $" {endTick.ToByteHexString()}";

            output += $" {entities.Count().ToByteHexString()}";
            foreach (var entity in entities)
                output += $" {entity.id.ToByteHexString()}";

            output += $" {spawns.Count().ToByteHexString()}";
            foreach (var entity in spawns)
                output += $" {entity.id.ToByteHexString()}";

            output += $" {despawns.Count().ToByteHexString()}";
            foreach (var entity in despawns)
                output += $" {entity.id.ToByteHexString()}";

            foreach (var componentType in State.componentTypes)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);

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
                        output += $" {skip.ToByteHexString()}";
                        output += $" {delta.ToByteHexString()}";
                        i++;
                        skip = 0;
                    }
                }
                if (skip > 0)
                    output += $" {skip.ToByteHexString()}";
            }
            var count = output.Replace(" ", "").Length / 2;
            return $"{output} [{count}]";
        }

        public IEnumerable<byte> ToBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(startTick.ToBytes());
            bytes.AddRange(endTick.ToBytes());

            bytes.AddRange(entities.Count().ToBytes());
            foreach (var entity in entities)
                bytes.AddRange(entity.id.ToBytes());

            bytes.AddRange(spawns.Count().ToBytes());
            foreach (var entity in spawns)
                bytes.AddRange(entity.id.ToBytes());

            bytes.AddRange(despawns.Count().ToBytes());
            foreach (var entity in despawns)
                bytes.AddRange(entity.id.ToBytes());

            foreach (var componentType in State.componentTypes)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);

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
                        bytes.AddRange(skip.ToBytes());
                        bytes.AddRange(delta.ToBytes());
                        i++;
                        skip = 0;
                    }
                }
                if (skip > 0)
                    bytes.AddRange(skip.ToBytes());
            }
            return bytes;
        }

        public class Change
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
