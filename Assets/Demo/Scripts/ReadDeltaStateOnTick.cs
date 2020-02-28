using EntityComponentState;
using System.IO;
using UnityEngine;
using static EntityComponentState.Constants;
using static TransformStateCompressed;

[RequireComponent(typeof(StateMB))]
public class ReadDeltaStateOnTick : MonoBehaviour
{
    public StateMB stateMB;
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();
    private FileStream tickFile;

    public float simulationUpdatesPerSecond => 1f / SIMULATION_RATE;

    private void OnValidate()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
    }

    private void OnEnable()
    {
        Time.fixedDeltaTime = simulationUpdatesPerSecond;
        tickFile = File.Open(TICK_FILE, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        //if (stateMB.state.tick > 0 && stateMB.state.tick % TICK_RATE == 0)
        {
            var bytes = new byte[tickFile.Length];
            tickFile.Read(bytes, 0, bytes.Length);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            var deltaState = deltaStateHistory.LatestDeltaState;

        }
    }
}