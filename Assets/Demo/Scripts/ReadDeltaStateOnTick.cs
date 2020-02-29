using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using UnityEngine;
using static TransformStateCompressed;

public class ReadDeltaStateOnTick : MonoBehaviour
{
    public int targetTick;
    public float currentTick;
    public float playbackSpeed = 0.1f;
    public TransformState state = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();
    public bool isJumpPressed = false;

    public float ticksPerFixedUpdate => 1f / Constants.TICK_RATE;
    public float simulationUpdatesPerSecond => 1f / Constants.SIMULATION_RATE;
    private string Path => Constants.TICK_FILE;

    private void OnEnable()
    {
        Time.fixedDeltaTime = simulationUpdatesPerSecond;
    }

    private void FixedUpdate()
    {
        if (currentTick >= state.tick)
        {
            var bytes = StateFile.ReadBytes(Path);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            if (targetTick < deltaStateHistory.Count)
            {
                var deltaState = deltaStateHistory[targetTick];
                if (deltaState != null)
                {
                    state = (TransformState)deltaState.GenerateEndState(state);
                    StateToScene.SpawnEntities(state, clones, transform);
                    StateToScene.DespawnEntities(state, clones);
                    StateToScene.ApplyChangesToEntites(state, clones);
                }
                targetTick++;
            }
        }
        currentTick += playbackSpeed * ticksPerFixedUpdate;
    }
}