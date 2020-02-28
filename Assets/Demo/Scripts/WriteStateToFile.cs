using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;

[RequireComponent(typeof(StateMB))]
public class WriteStateToFile : MonoBehaviour
{
    public StateMB stateMB;
    private FileStream file;

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
        file = File.Open(STATE_FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        file.SetLength(0);
    }

    private void OnDisable()
    {
        file.Close();
    }

    private void FixedUpdate()
    {
        try
        {
            file.Position = 0;
            var stateBytes = stateMB.state.ToBytes().ToArray();
            file.Write(stateBytes, 0, stateBytes.Length);
        }
        catch
        {
        }
    }
}
