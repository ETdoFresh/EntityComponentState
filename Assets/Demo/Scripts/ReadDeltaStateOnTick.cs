using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using UnityEngine;
using static TransformStateCompressed;

[RequireComponent(typeof(StateMB))]
public class ReadDeltaStateOnTick : MonoBehaviour
{
    public int currentTick = -1;
    public StateMB stateMB;
    public TransformState state = new TransformState();
    public List<StateClone> clones = new List<StateClone>(); 
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();

    public float simulationUpdatesPerSecond => 1f / Constants.SIMULATION_RATE;
    private string Path => Constants.TICK_FILE;

    private void OnValidate()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnEnable()
    {
        Time.fixedDeltaTime = simulationUpdatesPerSecond;
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        //if (stateMB.state.tick > 0 && stateMB.state.tick % TICK_RATE == 0)
        {
            var bytes = StateFile.ReadBytes(Path);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            currentTick = Mathf.Min(currentTick + 1, deltaStateHistory.LatestTick);
            var deltaState = deltaStateHistory.GetDeltaState(currentTick);
            if (deltaState != null)
            {
                state = (TransformState)deltaState.GenerateEndState(state);
                StateToScene.SpawnEntities(state, clones, transform);
                StateToScene.DespawnEntities(state, clones);
                StateToScene.ApplyChangesToEntites(state, clones);
            }
        }
    }
}