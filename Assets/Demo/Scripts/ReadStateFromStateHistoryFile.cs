using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TransformStateCompressed;

public class ReadStateFromStateHistoryFile : MonoBehaviour
{
    public int countPosition;
    public int count;
    public State state = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    private FileStream stateHistoryFile;
    public bool isPlaying = true;
    public bool isLive = false;
    public StateHistory<TransformState> stateHistory = new StateHistory<TransformState>();

    private string Path => Constants.STATEHISTORY_FILE;

    private void OnDisable()
    {
        StateFile.Close(Path);
    }

    private void FixedUpdate()
    {
        if (StateFile.HasChanged(Path))
        {
            var bytes = StateFile.ReadBytes(Path);
            stateHistory.FromBytes(new ByteQueue(bytes));
            count = stateHistory.LatestTick;
        }

        if (count > 0)
        {
            if (isLive && isPlaying)
                countPosition = count - 1;
            else
                countPosition = Math.Min(countPosition, count - 1);
            state = stateHistory.GetState(countPosition);
            if (state != null)
            {
                StateToScene.SpawnEntities(state, clones, transform);
                StateToScene.DespawnEntities(state, clones);
                StateToScene.ApplyChangesToEntites(state, clones);
            }
        }

        if (isPlaying)
            countPosition++;
    }
}
