using EntityComponentState;
using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;

public class WriteDeltaStateToFile : MonoBehaviour
{
    public StateMB stateMB;
    public State previousState;
    private FileStream deltaStateFile;

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
        deltaStateFile = File.Open(DELTASTATE_FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        deltaStateFile.SetLength(0);
    }

    private void OnDisable()
    {
        deltaStateFile.Close();
    }

    private void FixedUpdate()
    {
        var state = stateMB.state;
        try
        {
            if (previousState != null)
            {
                deltaStateFile.Position = 0;
                deltaStateFile.Write(stateMB.deltaState.ToBytes().ToArray(), 0, stateMB.deltaState.ToBytes().Count);
            }
        }
        catch
        {
        }
        previousState = state;
    }
}
