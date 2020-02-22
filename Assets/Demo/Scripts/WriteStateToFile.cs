using EntityComponentState.Unity;
using System.Collections;
using System.IO;
using UnityEngine;

public class WriteStateToFile : MonoBehaviour
{
    public const string FILE = @"D:\Desktop\EntityComponentState\state.bin";
    public StateMB stateMB;
    private FileStream file;

    private void OnEnable()
    {
        file = File.Open(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
    }

    private void OnDisable()
    {
        file.Close();
    }

    private void Update()
    {
        try
        {
            file.Position = 0;
            file.Write(stateMB.state.ToBytes().ToArray(), 0, stateMB.state.ToBytes().Count);
        }
        catch
        {
        }
    }
}
