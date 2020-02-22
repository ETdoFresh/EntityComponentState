using EntityComponentState.Unity;
using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AState))]
public class WriteStateToFile : MonoBehaviour
{
    public const string FILE = @"D:\Desktop\EntityComponentState\state.bin";
    public AState aState;
    private FileStream file;

    private void OnEnable()
    {
        if (!aState) aState = GetComponent<AState>();
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
            file.Write(aState.ToBytes().ToArray(), 0, aState.ToBytes().Count);
        }
        catch
        {
        }
    }
}
