using Common.Serializer.Abstraction;


namespace UserService.Serializer;

public class JsonSerializer : ISerializer
{
    public async Task<T> Deserialize<T>(byte[] value) where T : class
    {
        using var serialized = new MemoryStream(value);
        var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(serialized);

        if (result is null)
        {
            throw new Exception("Failed to Deserialize back to object, Sorry");
        }

        return result;
    }

    public async Task<byte[]> Serialize(object value)
    {
        using var serialized = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(serialized, value);

        if (serialized is null)
        {
            throw new Exception("Could not Serialize Data - Memory Came back empty");
        }

        return serialized.ToArray();
    }
}
