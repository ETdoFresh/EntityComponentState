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
    public TransformState startState = null;
    public TransformState endState = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();
    public bool isJumpPressed = false;

    public float ticksPerFixedUpdate => 1f / Constants.TICK_RATE;
    public float simulationUpdatesPerSecond => 1f / Constants.SIMULATION_RATE;
    private string Path => Constants.TICK_FILE;

    public float time;

    private void OnEnable()
    {
        Time.fixedDeltaTime = simulationUpdatesPerSecond;
    }

    private void Update()
    {
        if (startState != null)
        {
            var t = (currentTick - startState.tick) / (endState.tick - startState.tick);
            State state = startState;
            if (t <= 0) state = startState;
            else if (t >= 1) state = endState;
            else state = State.Lerp(startState, endState, t);
            StateToScene.SpawnEntities(state, clones, transform);
            StateToScene.DespawnEntities(state, clones);
            StateToScene.ApplyChangesToEntites(state, clones);
        }
    }

    private void FixedUpdate()
    {
        time = Time.time;
        if (Time.time > 0)
            currentTick += playbackSpeed;

        if (currentTick >= endState.tick)
        {
            var bytes = StateFile.ReadBytes(Path);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            if (targetTick < deltaStateHistory.Count)
            {
                var deltaState = deltaStateHistory[targetTick];
                if (deltaState != null)
                {
                    startState = endState;
                    endState = (TransformState)deltaState.GenerateEndState(endState);
                }
                targetTick++;
            }
        }
    }
}