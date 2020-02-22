using EntityComponentState;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class SerializableListBase<T> : List<T>, IToBytes where T : IToBytes
{
    public abstract ByteQueue ToBytes();
    public abstract void FromBytes(ByteQueue bytes);
}

public class SerializableList<T> : List<T>, IToBytes where T : IToBytes
{
    [NonSerialized] public Type type;

    public SerializableList()
    {
        type = typeof(T);
    }

    public virtual ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public virtual void FromBytes(ByteQueue bytes)
    {
        Clear();
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetIToBytes<T>(type));
    }
}

public class SerializableListByteCount<T> : SerializableList<T> where T : IToBytes
{
    public override ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue((byte)Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public override void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetByte();
        for (int i = 0; i < count; i++)
            Add(bytes.GetIToBytes<T>(type));
    }
}

public class SerializableListByte: List<byte>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetByte());
    }
}

public class SerializableListBool : List<bool>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetBool());
    }
}

public class SerializableListShort : List<short>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetShort());
    }
}

public class SerializableListUShort : List<ushort>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetUShort());
    }
}

public class SerializableListInt : List<int>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetInt());
    }
}

public class SerializableListUInt : List<uint>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetUInt());
    }
}

public class SerializableListFloat : List<float>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetFloat());
    }
}

public class SerializableListDouble : List<double>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetDouble());
    }
}

public class SerializableListString : List<string>, IToBytes
{
    public ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i]);
        return bytes;
    }

    public void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(bytes.GetString());
    }
}