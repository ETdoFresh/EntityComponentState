using EntityComponentState;
using System.IO;
using UnityEngine;

public class WriteStateHistoryToFile : MonoBehaviour
{
    public StateHistoryMB stateHistoryMB;
    public State previousState;

    private string Path => Constants.STATEHISTORY_FILE;

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

        var bytes = stateHistoryMB.stateHistory.LatestState.ToBytes().ToArray();
        StateFile.Write(Path, bytes);
    }
}
