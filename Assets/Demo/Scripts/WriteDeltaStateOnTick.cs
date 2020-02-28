using EntityComponentState;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;
using static TransformStateCompressed;

[RequireComponent(typeof(StateMB))]
public class WriteDeltaStateOnTick : MonoBehaviour
{
    public StateMB stateMB;
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();
    public TransformState previousState = new TransformState();
    private FileStream tickFile;

    public float simulationUpdatesPerSecond => 1f / SIMULATION_RATE;

    private void OnValidate()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnEnable()
    {
        Time.fixedDeltaTime = simulationUpdatesPerSecond;
        tickFile = File.Open(TICK_FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        tickFile.SetLength(0);
    }

    private void FixedUpdate()
    {
        if (stateMB.state.tick > 0 && stateMB.state.tick % TICK_RATE == 0)
        {
            var deltaState = new TransformDeltaState(previousState, stateMB.state);
            deltaStateHistory.Add(deltaState);

            if (tickFile.Length > 0) tickFile.Write(DELIMITER, 0, DELIMITER.Length);
            var bytes = deltaState.ToBytes().ToArray();
            tickFile.Write(bytes, 0, bytes.Length);

            previousState.tick = stateMB.state.tick;
            previousState.entities.Clear();
            previousState.entities.AddRange(stateMB.state.entities.Clone());
        }
    }
}
