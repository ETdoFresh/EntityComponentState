using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TransformStateCompressed;

public class ReadDeltaStateFromStateHistoryFile : MonoBehaviour
{
    public int tick;
    public int count;
    public bool isPlaying = true;
    public TransformState state = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();

    private string Path => Constants.DELTASTATEHISTORY_FILE;

    private void OnDisable()
    {
        StateFile.Close(Path);
    }

    private void FixedUpdate()
    {
        if (StateFile.HasChanged(Path))
        {
            var bytes = StateFile.ReadBytes(Path);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            count = deltaStateHistory.Count;
        }
        if (count > 0 && isPlaying)
        {
            var deltaState = deltaStateHistory.GetDeltaState(state.tick);
            if (deltaState != null)
            {
                state = (TransformState)deltaState.GenerateEndState(state);
                StateToScene.SpawnEntities(state, clones, transform);
                StateToScene.DespawnEntities(state, clones);
                StateToScene.ApplyChangesToEntites(state, clones);
            }
            tick = state.tick;
        }
    }
}