using EntityComponentState;
using System.IO;
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

    private void Update()
    {
        try
        {
            stateHistoryFile.Position = 0;
            stateHistoryFile.Write(stateHistoryMB.stateHistory.ToBytes().ToArray(), 0, stateHistoryMB.stateHistory.ToBytes().Count);
        }
        catch
        {
        }
    }
}
