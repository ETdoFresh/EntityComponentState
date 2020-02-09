using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class DeltaState
    {
        public State startState;
        public State targetState;
        public List<Entity> spawns = new List<Entity>();
        public List<Position> positions;
        //...
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
        }

        private void AddUpdates()
        {
            foreach(var entity in startState.entities.OrderBy(entity => entity.id))
            {
                if (entity.HasComponent<Position>())
                {
                }
            }
        }

        private void AddDespawns()
        {
            var continuingEntities = targetState.entities.Where(targetEntity => startState.entities.Any(startEntity => startEntity.id == targetEntity.id));
            var removedEntities = startState.entities.Except(continuingEntities);
            despawns.AddRange(removedEntities);
        }
    }
}
