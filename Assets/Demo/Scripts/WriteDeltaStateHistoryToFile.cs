using EntityComponentState;
using UnityEngine;

[RequireComponent(typeof(StateHistoryMB))]
public class WriteDeltaStateHistoryToFile : MonoBehaviour
{
    public StateHistoryMB stateHistoryMB;
    public State previousState;

    private string Path => Constants.DELTASTATEHISTORY_FILE;

    private void OnValidate()
    {
        if (!stateHistoryMB) stateHistoryMB = GetComponent<StateHistoryMB>();
    }

    private void OnEnable()
    {
        StateFile.OpenAndResetForWrite(Path);
    }

    private void OnDisable()
    {
        StateFile.Close(Path);
    }

    private void FixedUpdate()
    {
        if (!StateFile.IsEmpty(Path))
            StateFile.Write(Path, Constants.DELIMITER);

        var history = stateHistoryMB.deltaStateHistory;
        if (history.Count > 0)
        {
            var latestDeltaState = stateHistoryMB.deltaStateHistory[history.Count - 1];
            if (latestDeltaState.startStateTick != latestDeltaState.endStateTick)
            {
                var bytes = latestDeltaState.ToBytes().ToArray();
                StateFile.Write(Path, bytes);
            }
        }
    }
}
