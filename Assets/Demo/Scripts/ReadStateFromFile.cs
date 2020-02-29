using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMB))]
public class ReadStateFromFile : MonoBehaviour
{
    public StateMB stateMB;
    public List<StateClone> clones = new List<StateClone>();
    public ByteQueue byteQueue = new ByteQueue();

    private string Path => Constants.STATE_FILE;

    private void OnValidate()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnDisable()
    {
        StateFile.Close(Path);
    }

    private void Update()
    {
        var bytes = StateFile.ReadBytes(Path);
        stateMB.state.FromBytes(new ByteQueue(bytes));
        StateToScene.SpawnEntities(stateMB.state, clones, transform);
        StateToScene.DespawnEntities(stateMB.state, clones);
        StateToScene.ApplyChangesToEntites(stateMB.state, clones);
    }
}
