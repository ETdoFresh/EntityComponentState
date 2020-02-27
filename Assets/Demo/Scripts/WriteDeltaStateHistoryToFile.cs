using EntityComponentState;
using System.IO;
using UnityEngine;

public class WriteDeltaStateHistoryToFile : MonoBehaviour
{
    public const string FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaStateHistory.bin";
    public StateHistoryMB stateHistoryMB;
    public State previousState;
    private FileStream deltaStateHistoryFile;

    private void OnEnable()
    {
        if (!stateHistoryMB) stateHistoryMB = GetComponent<StateHistoryMB>();
        deltaStateHistoryFile = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
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
                deltaStateHistoryFile.Write(DeltaStateHistory.STATE_DELIMITER, 0, DeltaStateHistory.STATE_DELIMITER.Length);

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
