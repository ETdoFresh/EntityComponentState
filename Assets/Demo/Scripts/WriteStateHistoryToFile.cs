using EntityComponentState;
using System.IO;
using System.Text;
using UnityEngine;

public class WriteStateHistoryToFile : MonoBehaviour
{
    public const string FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\stateHistory.bin";
    public StateHistoryMB stateHistoryMB;
    public State previousState;
    private FileStream stateHistoryFile;

    private void OnEnable()
    {
        if (!stateHistoryMB) stateHistoryMB = GetComponent<StateHistoryMB>();
        stateHistoryFile = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
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
                stateHistoryFile.Write(StateHistory<State>.STATE_DELIMITER, 0, StateHistory<State>.STATE_DELIMITER.Length);

            var latestState = stateHistoryMB.stateHistory.LatestState.ToBytes().ToArray();
            stateHistoryFile.Write(latestState, 0, latestState.Length);
        }
        catch
        {
        }
    }
}
