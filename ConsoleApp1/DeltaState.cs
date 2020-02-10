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
