namespace Common.Serializer.Abstraction;

public interface ISerializer
{
    Task<byte[]> Serialize(object value);
    Task<T> Deserialize<T>(byte[] value) where T : class;
}
