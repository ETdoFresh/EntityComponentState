using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public class DeltaState : IToBytes
    {
        public State startState;
        public State endState;
        public IEnumerable<Entity> spawns;
        public IEnumerable<Entity> despawns;
        private IEnumerable<Change> changes;

        public DeltaState(State startState, State endState)
        {
            this.startState = startState;
            this.endState = endState;
            var startEntities = startState.entities;
            var endEntities = endState.entities;

            spawns = endEntities.Where(endEntity => !startEntities.Any(startEntity => startEntity.id == endEntity.id));
            despawns = startEntities.Where(startEntity => !endEntities.Any(endEntity => startEntity.id == endEntity.id));
            changes = new Change[0]; // TODO: Impement
        }

        public override string ToString()
        {
            var output = $"State [Start Tick: {startState.tick} End Tick: {endState.tick}]\r\n";

            //output += $"\r\nSpawns [Count: {spawns.Count()}]\r\n";
            //foreach (var entity in spawns)
            //    output += $"{entity.id} ";

            //output += $"\r\nDespawns [Count: {despawns.Count()}]\r\n";
            //foreach (var entity in despawns)
            //    output += $"{entity.id} ";

            output += "\r\n";

            foreach (var componentType in endState.types)
            {
                //var componentChanges = changes.Where(change => change.componentType == componentType);
                //output += $"  {componentType.Name} [Count: {componentChanges.Count()}]\r\n";
                //foreach (var component in componentChanges.Select(change => change.delta))
                //    if (component is null)
                //        output += $"    SKIP\r\n";
                //    else
                //        output += $"    {component}\r\n";
            }

            return output;
        }

        public ByteQueue ToBytes()
        {
            var bytes = new ByteQueue();
            bytes.Enqueue(startState.tick);
            bytes.Enqueue(endState.tick);

            bytes.Enqueue(spawns.Count());
            foreach (var entity in spawns)
                bytes.Enqueue(entity.id);

            bytes.Enqueue(despawns.Count());
            foreach (var entity in despawns)
                bytes.Enqueue(entity.id);

            foreach (var componentType in endState.types)
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
