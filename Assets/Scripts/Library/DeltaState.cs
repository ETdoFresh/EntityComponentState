using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public class DeltaState
    {
        public State startState;
        public State endState;
        public int tick;
        public List<Entity> spawns = new List<Entity>();
        private readonly List<Change> changes = new List<Change>();
        public List<Entity> despawns = new List<Entity>();
        public List<Type> types;

        public DeltaState(State startState, State endState)
        {
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
            this.startState = startState;
            this.endState = endState;
            tick = endState.tick;
            var startEntities = startState.entities;
            var endEntities = endState.entities;

            var addedEntities = endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id));
            var removedEntities = startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id));

            spawns.AddRange(addedEntities);
            despawns.AddRange(removedEntities);
            AddUpdates();
        }

        public DeltaState(byte[] bytes, State startState)
        {
            this.startState = startState;
            this.endState = new State();
            var byteQueue = new ByteQueue(bytes);
            var startTick = byteQueue.GetInt();
            endState.tick = byteQueue.GetInt();
            tick = endState.tick;

            var spawnCount = byteQueue.GetInt();
            for (int i = 0; i < spawnCount; i++)
                spawns.Add(new Entity(byteQueue.GetInt()));

            var despawnCount = byteQueue.GetInt();
            for (int i = 0; i < despawnCount; i++)
                despawns.Add(startState.entities.Where(entity => entity.id == byteQueue.GetInt()).First());

            endState.entities.AddRange(startState.entities.Union(spawns).Except(despawns).OrderBy(entity => entity.id));

            foreach (var componentType in types)
            {
                var currentIndex = 0;
                while (currentIndex < endState.entities.Count)
                {
                    var skip = byteQueue.GetInt();
                    for (int i = 0; i < skip; i++)
                        changes.Add(new Change { componentType = componentType, entityId = endState.entities[currentIndex + i].id });
                    currentIndex += skip;
                    if (currentIndex >= endState.entities.Count) break;

                    var entity = endState.entities[currentIndex];
                    var component = (Component)Activator.CreateInstance(componentType);
                    entity.AddComponent(component);
                    component.Deserialize(byteQueue);
                    changes.Add(new Change { componentType = componentType, entityId = entity.id, delta = component });
                    currentIndex++;
                }
            }
        }

        public State GenerateEndState()
        {
            var endState = new State
            {
                tick = tick
            };
            endState.entities.AddRange(startState.entities.Union(spawns).Except(despawns).OrderBy(entity => entity.id));
            foreach (var componentType in types)
                foreach (var entity in endState.entities)
                {
                    var change = changes.Where(c => c.entityId == entity.id && c.componentType == componentType).FirstOrDefault();
                    if (change.delta != null)
                        if (entity.HasComponent(componentType))
                            entity.GetComponent(componentType).CopyValuesFrom(change.delta);
                        else
                            entity.AddComponent(change.delta);
                }
            return endState;
        }

        private void AddUpdates()
        {
            foreach (var componentType in types)
                foreach (var endStateEntity in endState.entities)
                {
                    var change = new Change { componentType = componentType, entityId = endStateEntity.id };
                    var startStateEntity = startState.entities.Where(sse => sse.id == endStateEntity.id).FirstOrDefault();
                    if (startStateEntity == null)
                    {
                        if (endStateEntity.HasComponent(componentType))
                        {
                            change.delta = endStateEntity.GetComponent(componentType);
                        }
                        // Else Skip
                    }
                    else // StartState has entity
                    {
                        if (endStateEntity.GetComponent(componentType) != startStateEntity.GetComponent(componentType))
                        {
                            change.delta = endStateEntity.GetComponent(componentType);
                        }
                        // Else Skip
                    }
                    changes.Add(change);
                }
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

            foreach (var componentType in types)
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

        public string ToCompressedByteHexString()
        {
            var output = "";
            output += startState.tick.ToByteHexString();
            output += $" {endState.tick.ToByteHexString()}";

            output += $" {((byte)spawns.Count()).ToByteHexString()}";
            foreach (var entity in spawns)
                output += $" {((byte)entity.id).ToByteHexString()}";

            output += $" {((byte)despawns.Count()).ToByteHexString()}";
            foreach (var entity in despawns)
                output += $" {((byte)entity.id).ToByteHexString()}";

            foreach (var componentType in types)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);

                var i = 0;
                byte skip = 0;
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
                        output += $" {delta.ToCompressedByteHexString()}";
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

        public string ToByteHexString()
        {
            var output = "";
            output += startState.tick.ToByteHexString();
            output += $" {endState.tick.ToByteHexString()}";

            output += $" {spawns.Count().ToByteHexString()}";
            foreach (var entity in spawns)
                output += $" {entity.id.ToByteHexString()}";

            output += $" {despawns.Count().ToByteHexString()}";
            foreach (var entity in despawns)
                output += $" {entity.id.ToByteHexString()}";

            foreach (var componentType in types)
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

        public byte[] ToBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(startState.tick.ToBytes());
            bytes.AddRange(endState.tick.ToBytes());

            bytes.AddRange(spawns.Count().ToBytes());
            foreach (var entity in spawns)
                bytes.AddRange(entity.id.ToBytes());

            bytes.AddRange(despawns.Count().ToBytes());
            foreach (var entity in despawns)
                bytes.AddRange(entity.id.ToBytes());

            foreach (var componentType in types)
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
            return bytes.ToArray();
        }

        public byte[] ToCompressedBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(startState.tick.ToBytes());
            bytes.AddRange(endState.tick.ToBytes());

            bytes.AddRange(((byte)spawns.Count()).ToBytes());
            foreach (var entity in spawns)
                bytes.AddRange(((byte)entity.id).ToBytes());

            bytes.AddRange(((byte)despawns.Count()).ToBytes());
            foreach (var entity in despawns)
                bytes.AddRange(((byte)entity.id).ToBytes());

            foreach (var componentType in types)
            {
                var componentChanges = changes.Where(change => change.componentType == componentType);

                var i = 0;
                byte skip = 0;
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
                        bytes.AddRange(delta.ToCompressedBytes());
                        i++;
                        skip = 0;
                    }
                }
                if (skip > 0)
                    bytes.AddRange(skip.ToBytes());
            }
            return bytes.ToArray();
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
