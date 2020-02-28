using EntityComponentState;
using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;

public class WriteDeltaStateHistoryToFile : MonoBehaviour
{
    public StateHistoryMB stateHistoryMB;
    public State previousState;
    private FileStream deltaStateHistoryFile;

    private void OnEnable()
    {
        if (!stateHistoryMB) stateHistoryMB = GetComponent<StateHistoryMB>();
        deltaStateHistoryFile = File.Open(DELTASTATEHISTORY_FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        deltaStateHistoryFile.SetLength(0);
    }

    private void OnDisable()
    {
        deltaStateHistoryFile.Close();
    }

    private void FixedUpdate()
    {
        try
        {
            if (deltaStateHistoryFile.Length > 0)
                deltaStateHistoryFile.Write(DELIMITER, 0, DELIMITER.Length);

            var latestDeltaState = stateHistoryMB.deltaStateHistory.LatestDeltaState;
            if (latestDeltaState.startStateTick != latestDeltaState.endStateTick)
            {
                var latestDeltaStateBytes = latestDeltaState.ToBytes().ToArray();
                deltaStateHistoryFile.Write(latestDeltaStateBytes, 0, latestDeltaStateBytes.Length);
            }
        }
        catch
        {
        }
    }
}
