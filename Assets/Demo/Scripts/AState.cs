using EntityComponentState;
using UnityEngine;

/// <summary>
/// Temporary Monobehaviour State base class. Should be StateMB I believe.
/// </summary>
public abstract class AState : MonoBehaviour
{
    public virtual State state { get; protected set; }
    public virtual DeltaState deltaState { get; protected set; }
    public abstract ByteQueue ToBytes();
    public abstract void FromBytes(ByteQueue bytes);
}
