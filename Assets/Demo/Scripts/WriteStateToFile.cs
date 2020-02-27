using EntityComponentState.Unity;
using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(StateMB))]
public class WriteStateToFile : MonoBehaviour
{
    public const string FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\state.bin";
    public StateMB stateMB;
    private FileStream file;

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
        file = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
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
