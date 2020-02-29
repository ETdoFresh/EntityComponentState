using System.Collections.Generic;
using System.IO;

public static class StateFile
{
    public static Dictionary<string, FileStream> fileStreams = new Dictionary<string, FileStream>();
    public static Dictionary<string, byte[]> fileBytes = new Dictionary<string, byte[]>();

    public static void Close(string path)
    {
        if (fileStreams.ContainsKey(path))
            fileStreams[path].Close();

        fileBytes.Remove(path);
        fileStreams.Remove(path);
    }

    public static void OpenForRead(string path)
    {
        if (!fileBytes.ContainsKey(path)) fileBytes.Add(path, System.Array.Empty<byte>());
        if (!fileStreams.ContainsKey(path))
            fileStreams.Add(path, File.Open(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
    }

    public static byte[] ReadBytes(string path)
    {
        if (!fileStreams.ContainsKey(path))
            OpenForRead(path);

        var file = fileStreams[path];
        if (file.Length != fileBytes[path].Length)
        {
            fileBytes[path] = new byte[file.Length];
            file.Position = 0;
            file.Read(fileBytes[path], 0, (int)file.Length);
        }
        return fileBytes[path];
    }

    public static bool HasChanged(string path)
    {
        if (!fileStreams.ContainsKey(path))
            OpenForRead(path);

        return fileStreams[path].Length != fileBytes[path].Length;
    }

    public static void OpenForWrite(string path)
    {
        if (!fileBytes.ContainsKey(path)) fileBytes.Add(path, System.Array.Empty<byte>());
        if (!fileStreams.ContainsKey(path))
            fileStreams.Add(path, File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));

    }

    public static void OpenAndResetForWrite(string path)
    {
        OpenForWrite(path);
        fileStreams[path].SetLength(0);
    }

    public static void Write(string path, byte[] bytes)
    {
        if (!fileStreams.ContainsKey(path))
            OpenForWrite(path);

        fileStreams[path].Write(bytes, 0, bytes.Length);
    }

    public static void ResetAndWrite(string path, byte[] bytes)
    {
        if (!fileStreams.ContainsKey(path))
            OpenAndResetForWrite(path);
        else
            //fileStreams[path].SetLength(0); // Slower but actually reduces file 
            fileStreams[path].Position = 0;
        

        fileStreams[path].Write(bytes, 0, bytes.Length);
    }

    public static bool IsEmpty(string path)
    {
        if (fileStreams.ContainsKey(path))
            return fileStreams[path].Length == 0;
        else
            return true;
    }
}