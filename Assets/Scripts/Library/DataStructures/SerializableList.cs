using EntityComponentState;
using System;
using System.Collections.Generic;

public abstract class SerializableListBase<T> : List<T>, IToBytes where T : IToBytes
{
    public abstract ByteQueue ToBytes();
    public abstract void FromBytes(ByteQueue bytes);
}

public class SerializableList<T> : List<T>, IToBytes where T : IToBytes
{
    [NonSerialized] public Type type;

    public SerializableList() { type = typeof(T); }
    public SerializableList(IEnumerable<T> collection) : base(collection) { type = typeof(T); }

    public virtual SerializableList<T> Clone() => new SerializableList<T>(this);

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
    public SerializableListByteCount() { type = typeof(T); }
    public SerializableListByteCount(IEnumerable<T> collection) : base(collection) { }

    public override SerializableList<T> Clone() => new SerializableListByteCount<T>(this);

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

public class SerializableListByte : List<byte>, IToBytes
{
    public SerializableListByte() : base() { }
    public SerializableListByte(IEnumerable<byte> collection) : base(collection) { }

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
    public SerializableListBool() : base() { }
    public SerializableListBool(IEnumerable<bool> collection) : base(collection) { }

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
    public SerializableListShort() : base() { }
    public SerializableListShort(IEnumerable<short> collection) : base(collection) { }

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
    public SerializableListUShort() : base() { }
    public SerializableListUShort(IEnumerable<ushort> collection) : base(collection) { }

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
    public SerializableListInt() : base() { }
    public SerializableListInt(IEnumerable<int> collection) : base(collection) { }

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
    public SerializableListUInt() : base() { }
    public SerializableListUInt(IEnumerable<uint> collection) : base(collection) { }

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
    public SerializableListFloat() : base() { }
    public SerializableListFloat(IEnumerable<float> collection) : base(collection) { }

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
    public SerializableListDouble() : base() { }
    public SerializableListDouble(IEnumerable<double> collection) : base(collection) { }

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
    public SerializableListString() : base() { }
    public SerializableListString(IEnumerable<string> collection) : base(collection) { }

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

public class SerializableListEntity : List<Entity>, IToBytes
{
    public SerializableListEntity() : base() { }
    public SerializableListEntity(IEnumerable<Entity> collection) : base(collection) { }

    public virtual SerializableListEntity Clone() => new SerializableListEntity(this);

    public virtual ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue(Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue(this[i].id);
        return bytes;
    }

    public virtual void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetInt();
        for (int i = 0; i < count; i++)
            Add(new Entity(bytes.GetInt()));
    }
}

public class SerializableListEntityCompressed : SerializableListEntity
{
    public SerializableListEntityCompressed() : base() { }
    public SerializableListEntityCompressed(IEnumerable<Entity> collection) : base(collection) { }

    public override SerializableListEntity Clone() => new SerializableListEntityCompressed(this);

    public override ByteQueue ToBytes()
    {
        var bytes = new ByteQueue();
        bytes.Enqueue((byte)Count);
        for (int i = 0; i < Count; i++)
            bytes.Enqueue((byte)this[i].id);
        return bytes;
    }

    public override void FromBytes(ByteQueue bytes)
    {
        var count = bytes.GetByte();
        for (int i = 0; i < count; i++)
            Add(new Entity(bytes.GetByte()));
    }
}