using EntityComponentState;
using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;

public class WriteStateHistoryToFile : MonoBehaviour
{
    public StateHistoryMB stateHistoryMB;
    public State previousState;
    private FileStream stateHistoryFile;

    private void OnEnable()
    {
        if (!stateHistoryMB) stateHistoryMB = GetComponent<StateHistoryMB>();
        stateHistoryFile = File.Open(STATEHISTORY_FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        stateHistoryFile.SetLength(0);
    }

    private void OnDisable()
    {
        stateHistoryFile.Close();
    }

    private void FixedUpdate()
    {
        try
        {
            if (stateHistoryFile.Length > 0)
                stateHistoryFile.Write(DELIMITER, 0, DELIMITER.Length);

            var latestState = stateHistoryMB.stateHistory.LatestState.ToBytes().ToArray();
            stateHistoryFile.Write(latestState, 0, latestState.Length);
        }
        catch
        {
        }
    }
}
