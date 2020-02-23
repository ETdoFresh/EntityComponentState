using EntityComponentState;
using System.IO;
using UnityEngine;

public class WriteDeltaStateToFile : MonoBehaviour
{
    // aState.state is the current state
    // Write full state at start
    // Delta from either startState or lastReceivedState (latter perferred)

    public const string FILE_FULL_STATE = @"C:\Users\etgarcia\Desktop\EntityComponentState\lastReceivedState.bin";
    public const string FILE_DELTA_STATE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaState.bin";
    public AState aState;
    private State previousState;
    private FileStream lastReceivedStateFile;
    private FileStream deltaStateFile;

    private void OnEnable()
    {
        if (!aState) aState = GetComponent<AState>();
        previousState = aState.state.Clone();
        try
        {
            lastReceivedStateFile = File.Open(FILE_FULL_STATE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            lastReceivedStateFile.Write(aState.ToBytes().ToArray(), 0, aState.ToBytes().Count);
            lastReceivedStateFile.Close();
        }
        catch { }
        lastReceivedStateFile = File.Open(FILE_FULL_STATE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        deltaStateFile = File.Open(FILE_DELTA_STATE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

    }

    private void OnDisable()
    {
        lastReceivedStateFile.Close();
        deltaStateFile.Close();
    }

    private void Update()
    {
        try
        {
            var bytes = new byte[lastReceivedStateFile.Length];
            lastReceivedStateFile.Position = 0;
            lastReceivedStateFile.Read(bytes, 0, (int)lastReceivedStateFile.Length);
            previousState.FromBytes(new ByteQueue(bytes));

            var state = aState.state;
            var deltaState = new DeltaState(previousState, state);
            deltaStateFile.Position = 0;
            deltaStateFile.Write(deltaState.ToBytes().ToArray(), 0, deltaState.ToBytes().Count);
        }
        catch
        {
        }
    }
}
