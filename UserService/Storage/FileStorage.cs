using USDB.Storage.Abstraction;

namespace USDB.Storage;

internal class FileStorage(string directory, int sizeLimit) : IStorage
{
    public long Size { get; private set; } = 0;

    public async Task<bool> AddObject(string id, byte[] data, bool shouldOverride = false)
    {
        var sizeLimitBytes = (long) sizeLimit * 1_000_000;
        var fullPath = Path.Combine(Path.GetFullPath(directory), $"{id}.bin");
        var isExists = File.Exists(fullPath);

        if (isExists && !shouldOverride)
        {
            throw new Exception($"Could not override existing object {fullPath}");
        }

        if(Size + data.Length >= sizeLimitBytes)
        {
            throw new Exception($"File too big, size: {data.Length}B, remaining: {sizeLimitBytes - Size}B");
        }

        using var file = File.OpenWrite(fullPath);
        // in case of override
        Size -= file.Length;
        file.SetLength(0);

        Size += data.Length;
        await file.WriteAsync(data);

        return true;
    }

    public Task<bool> DeleteObject(string id)
    {
        try
        {
            var fullPath = Path.Combine(Path.GetFullPath(directory), $"{id}.bin");
            using var file = File.Open(fullPath, FileMode.Open);
            Size -= file.Length;
            File.Delete(fullPath);
            return Task.FromResult(true);
        }
        catch (Exception _)
        {
            return Task.FromResult(false);
        }
    }

    public void Dispose()
    {
        var fullPath = Path.GetFullPath(directory);

        foreach (var file in Directory.GetFiles(fullPath))
        {
            File.Delete(file);
        }

        Size = 0;
    }

    public async Task<byte[]?> GetObject(string id)
    {
        var fullPath = Path.Combine(Path.GetFullPath(directory), $"{id}.bin");
        var isExists = File.Exists(fullPath);

        if (!isExists) return null;

        using var file = File.OpenRead(fullPath);
        using var memstream = new MemoryStream();

        await file.CopyToAsync(memstream);

        return memstream.ToArray();
    }
}
