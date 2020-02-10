using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class DeltaState
    {
        public State startState;
        public State targetState;
        public IEnumerable<Entity> currentEntities;
        public List<Entity> spawns = new List<Entity>();
        public List<Change> changes = new List<Change>();
        public List<Entity> despawns = new List<Entity>();

        public DeltaState(State startState, State targetState)
        {
            this.startState = startState;
            this.targetState = targetState;

            AddSpawns();
            AddUpdates();
            AddDespawns();
        }

        private void AddSpawns()
        {
            var continuingEntities = targetState.entities.Where(targetEntity => startState.entities.Any(startEntity => startEntity.id == targetEntity.id));
            var newEntities = targetState.entities.Except(continuingEntities);
            spawns.AddRange(newEntities);
            currentEntities = startState.entities.Union(spawns).OrderBy(entity => entity.id);
        }

        private void AddUpdates()
        {
            foreach (var componentType in State.componentTypes)
                foreach (var entity in currentEntities)
                {
                    var change = new Change { componentType = componentType, entityId = entity.id };
                    var targetEntity = targetState.entities.Where(targetEntity => targetEntity.id == entity.id).FirstOrDefault();
                    if (!startState.entities.Contains(entity))
                    {
                        if (entity.HasComponent(componentType))
                        {
                            change.delta = entity.GetComponent(componentType);
                        }
                    }
                    else if (targetEntity != null)
                    {
                        if (entity.GetComponent(componentType) != targetEntity.GetComponent(componentType))
                        {
                            change.delta = targetEntity.GetComponent(componentType);
                        }
                    }
                    // Otherwise delta == null aka Skip
                    changes.Add(change);
                }
        }

        private void AddDespawns()
        {
            var continuingEntities = targetState.entities.Where(targetEntity => startState.entities.Any(startEntity => startEntity.id == targetEntity.id));
            var removedEntities = startState.entities.Except(continuingEntities);
            despawns.AddRange(removedEntities);
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startState.tick} Target Tick: {targetState.tick}]\r\n";
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
            output += startState.tick.ToByteHexString();
            output += $" {targetState.tick.ToByteHexString()}";

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
