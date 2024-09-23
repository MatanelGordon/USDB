using Localos.Storage.Abstraction;

namespace Localos.Storage;

internal class MemoryStorage(long limit) : IStorage
{
    public long Size { get; private set; } = 0;

    private readonly Dictionary<string, byte[]> _storage = new();

    public Task<bool> AddObject(string id, byte[] data, bool shouldOverride = false)
    {
        var limitInBytes = limit * 1_000_000;

        if (_storage.ContainsKey(id) && !shouldOverride)
        {
            throw new Exception($"Could not override existing object {id}");
        }

        if (Size + data.Length >= limitInBytes)
        {
            throw new Exception($"File too big, size: {data.Length}B, remaining: {limitInBytes - Size}B");
        }

        _storage.Add(id, data);
        Size += data.Length;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteObject(string id)
    {
        if(!_storage.ContainsKey(id)) return Task.FromResult(false);

        Size -= _storage[id].Length;
        _storage.Remove(id);
        return Task.FromResult(true);
    }

    public Task<byte[]?> GetObject(string id)
    {
        _storage.TryGetValue(id, out var result);
        return Task.FromResult(result);
    }

    public void Dispose()
    {
        _storage.Clear();
        Size = 0;
    }
}
