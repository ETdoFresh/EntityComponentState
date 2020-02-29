using EntityComponentState;
using UnityEngine;

[RequireComponent(typeof(StateMB))]
public class WriteStateToFile : MonoBehaviour
{
    public StateMB stateMB;

    private string Path => Constants.STATE_FILE;

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnDisable()
    {
        StateFile.Close(Path);
    }

    private void FixedUpdate()
    {
        var stateBytes = stateMB.state.ToBytes().ToArray();
        StateFile.ResetAndWrite(Path, stateBytes);
    }
}
