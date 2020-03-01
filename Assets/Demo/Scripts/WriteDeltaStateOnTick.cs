using EntityComponentState;
using UnityEngine;

using static TransformStateCompressed;

[RequireComponent(typeof(StateMB))]
public class WriteDeltaStateOnTick : MonoBehaviour
{
    public StateMB stateMB;
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();
    public TransformState previousState = new TransformState();

    public float SimulationUpdatesPerSecond => 1f / Constants.SIMULATION_RATE;
    private string Path => Constants.TICK_FILE;

    public float time;

    private void OnValidate()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnEnable()
    {
        Time.fixedDeltaTime = SimulationUpdatesPerSecond;
        StateFile.OpenAndResetForWrite(Path);
    }

    private void FixedUpdate()
    {
        time = Time.time;
        if (stateMB.state.tick > 0 && stateMB.state.tick % Constants.TICK_RATE == 0)
        {
            var deltaState = new TransformDeltaState();
            deltaState.Create(previousState, stateMB.state);
            deltaStateHistory.Add(deltaState);

            if (!StateFile.IsEmpty(Path))
                StateFile.Write(Path, Constants.DELIMITER);

            var bytes = deltaState.ToBytes().ToArray();
            StateFile.Write(Path, bytes);

            previousState.tick = stateMB.state.tick;
            previousState.entities.Clear();
            previousState.entities.AddRange(stateMB.state.entities.Clone());
        }
    }
}
