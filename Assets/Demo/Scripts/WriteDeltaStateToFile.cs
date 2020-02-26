using EntityComponentState;
using System.IO;
using UnityEngine;

public class WriteDeltaStateToFile : MonoBehaviour
{
    public const string FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaState.bin";
    public StateMB aState;
    public State previousState;
    private FileStream deltaStateFile;

    private void OnEnable()
    {
        if (!aState) aState = GetComponent<StateMB>();
        deltaStateFile = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        deltaStateFile.SetLength(0);
    }

    private void OnDisable()
    {
        deltaStateFile.Close();
    }

    private void Update()
    {
        var state = aState.state;
        try
        {
            if (previousState != null)
            {
                deltaStateFile.Position = 0;
                deltaStateFile.Write(aState.deltaState.ToBytes().ToArray(), 0, aState.deltaState.ToBytes().Count);
            }
        }
        catch
        {
        }
        previousState = state;
    }
}
