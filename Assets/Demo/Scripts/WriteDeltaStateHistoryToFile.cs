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
                deltaStateHistoryFile.Write(StateHistory.STATE_DELIMITER, 0, StateHistory.STATE_DELIMITER.Length);

            var latestDeltaState = stateHistoryMB.stateHistory.LatestDeltaState.ToBytes().ToArray();
            deltaStateHistoryFile.Write(latestDeltaState, 0, latestDeltaState.Length);
        }
        catch
        {
        }
    }
}
