using Microsoft.Extensions.Options;
using UserService.Config;
using UserService.Storage.Abstraction;

namespace UserService.Storage;

internal class StorageRouter(FileStorage fileStorage, MemoryStorage memoryStorage, long switchLimit = 2_500_000)
    : IStorage
{
    private readonly Dictionary<string, IStorage> _router = new();

    public StorageRouter(IOptions<DBConfigSchema> config, long switchLimit = 2_500_000) : this(
        new FileStorage(config.Value.Directory, config.Value.Limit),
        new MemoryStorage(config.Value.MemoryStorageLimit),
        switchLimit)
    {
    }

    public Task<bool> AddObject(string id, byte[] data, bool shouldOverride)
    {
        IStorage storage = data.Length > switchLimit ? fileStorage : memoryStorage;

        try
        {
            _router[id] = storage;
            return storage.AddObject(id, data);
        }
        catch (Exception _)
        {
            IStorage other = storage == fileStorage ? memoryStorage : fileStorage;

            try
            {
                _router[id] = other;
                return storage.AddObject(id, data);
            }
            catch (Exception __)
            {
                _router.Remove(id);
                throw;
            }
        }
    }

    public Task<bool> DeleteObject(string id)
    {
        var storage = _router[id];
        _router.Remove(id);
        return storage.DeleteObject(id);
    }

    public Task<byte[]?> GetObject(string id)
    {
        var storage = _router[id];
        return storage.GetObject(id);
    }

    public void Dispose()
    {
        fileStorage.Dispose();
        memoryStorage.Dispose();
        _router.Clear();
    }
}