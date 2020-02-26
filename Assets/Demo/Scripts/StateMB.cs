using EntityComponentState;
using UnityEngine;

public abstract class StateMB : MonoBehaviour
{
    public virtual State state { get; protected set; }
    public virtual DeltaState deltaState { get; protected set; }
    public abstract ByteQueue ToBytes();
    public abstract void FromBytes(ByteQueue bytes);
}
