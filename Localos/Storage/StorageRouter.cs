using Localos.Storage.Abstraction;
using Microsoft.Extensions.Options;

namespace Localos.Storage;

internal class StorageRouter : IStorage
{
    private readonly Dictionary<string, IStorage> _router = new();
    private readonly IStorage _fileStorage;
    private readonly IStorage _memoryStorage;
    private readonly long _switchLimit;

    public StorageRouter(FileStorage fileStorage, MemoryStorage memoryStorage, long switchLimit = 2_500_000)
    {
        _switchLimit = switchLimit;
        _fileStorage = fileStorage;
        _memoryStorage = memoryStorage;
    }

    public StorageRouter(IOptions<ConfigSchema> config, long switchLimit = 2_500_000)
    {
        _switchLimit = switchLimit;
        _fileStorage = new FileStorage(config.Value.Directory, config.Value.Limit);
        _memoryStorage = new MemoryStorage(config.Value.MemoryStorageLimit);
    }

    public Task<bool> AddObject(string id, byte[] data, bool shouldOverride)
    {
        IStorage storage = data.Length > _switchLimit ? _fileStorage : _memoryStorage;

        try
        {
            _router[id] = storage;
            return storage.AddObject(id, data);
        }
        catch (Exception _)
        {
            IStorage other = storage == _fileStorage ? _memoryStorage : _fileStorage;

            try
            {
                _router[id] = storage;
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
        IStorage storage = _router[id];
        _router.Remove(id);
        return storage.DeleteObject(id);
    }

    public Task<byte[]?> GetObject(string id)
    {
        IStorage storage = _router[id];
        return storage.GetObject(id);
    }

    public void Dispose()
    {
        _fileStorage.Dispose();
        _memoryStorage.Dispose();
        _router.Clear();
    }
}
