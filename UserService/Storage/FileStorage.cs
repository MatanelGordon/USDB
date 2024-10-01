using UserService.Storage.Abstraction;

namespace UserService.Storage;

internal class FileStorage(string directory, int sizeLimit) : IStorage
{
    protected long Size { get; private set; }
    protected List<string> Files { get; } = new();
    
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

        await using var file = File.OpenWrite(fullPath);
        // in case of override
        Size -= file.Length;
        file.SetLength(0);

        Size += data.Length;
        await file.WriteAsync(data);
        Files.Add(id);

        return true;
    }

    public Task<bool> DeleteObject(string id)
    {
        try
        {
            var fullPath = Path.Combine(Path.GetFullPath(directory), $"{id}.bin");
            using var file = File.Open(fullPath, FileMode.Open);
            File.Delete(fullPath);
            Size -= file.Length;
            Files.Remove(id);
            return Task.FromResult(true);
        }
        catch (Exception _)
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> ObjectExists(string id) => Task.Run(() => Files.Contains(id));

    public void Dispose()
    {
        var fullPath = Path.GetFullPath(directory);

        foreach (var file in Directory.GetFiles(fullPath))
        {
            File.Delete(file);
        }
        
        Files.Clear();
        Size = 0;
    }

    public async Task<byte[]?> GetObject(string id)
    {
        var fullPath = Path.Combine(Path.GetFullPath(directory), $"{id}.bin");
        var isExists = File.Exists(fullPath);

        if (!isExists) return null;

        await using var file = File.OpenRead(fullPath);
        await using var memstream = new MemoryStream();

        await file.CopyToAsync(memstream);

        return memstream.ToArray();
    }

    public void LoadExisting()
    {
        var files = Directory.GetFiles(Path.GetFullPath(directory));
        var totalSize = 0L;
        
        foreach (var path in files)
        {
            var info = new FileInfo(path);
            totalSize += info.Length;
        }

        if (totalSize <= sizeLimit)
        {
            Size += totalSize;
            return;
        }

        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}
