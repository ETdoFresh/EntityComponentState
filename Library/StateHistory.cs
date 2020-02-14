using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public class StateHistory : List<State>
    {
        public int LatestTick => this.Max(state => state.tick);
        public State LatestState => this.FirstOrDefault(state => state.tick == LatestTick);

        public State GetState(int tick)
        {
            return this.Where(state => state.tick == tick).FirstOrDefault();
        }

        public DeltaState GetDeltaState(int startTick, int endTick)
        {
            var startState = GetState(startTick);
            var endState = GetState(endTick);
            return new DeltaState(startState, endState);
        }

        public DeltaState GetDeltaState(int startTick)
        {
            return GetDeltaState(startTick, LatestTick);
        }
    }
}
