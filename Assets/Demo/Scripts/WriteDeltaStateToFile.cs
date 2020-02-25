using EntityComponentState;
using System.IO;
using UnityEngine;

public class WriteDeltaStateToFile : MonoBehaviour
{
    // aState.state is the current state
    // Write full state at start
    // Delta from either startState or lastReceivedState (latter perferred)

    public const string FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaState.bin";
    public AState aState;
    public State previousState;
    private FileStream deltaStateFile;

    private void OnEnable()
    {
        if (!aState) aState = GetComponent<AState>();
        deltaStateFile = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
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
