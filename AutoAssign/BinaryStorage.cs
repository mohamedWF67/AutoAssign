using System.IO;
using MessagePack;

public static class BinaryStorage
{
    public static void Save<T>(string filePath, T data)
    {
        var bytes = MessagePackSerializer.Serialize(data);
        File.WriteAllBytes(filePath, bytes);
    }

    public static T Load<T>(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        return MessagePackSerializer.Deserialize<T>(bytes);
    }
}