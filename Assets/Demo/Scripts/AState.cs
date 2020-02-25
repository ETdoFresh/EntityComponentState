using EntityComponentState;
using UnityEngine;

/// <summary>
/// Temporary Monobehaviour State base class. Should be StateMB I believe.
/// </summary>
public abstract class AState : MonoBehaviour
{
    public State state;
    public DeltaState deltaState;
    public abstract ByteQueue ToBytes();
    public abstract void FromBytes(ByteQueue bytes);
}
