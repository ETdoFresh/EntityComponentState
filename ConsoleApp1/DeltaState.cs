using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
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

        // Possibilities
        
        // State Empty  Removed? (Skip?)
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
