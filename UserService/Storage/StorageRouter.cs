using Microsoft.Extensions.Options;
using UserService.Config;
using UserService.Storage.Abstraction;

namespace UserService.Storage;

internal class StorageRouter
    : IStorage
{
    private readonly FileStorage _fileStorage;
    private readonly MemoryStorage _memoryStorage;
    private readonly long _switchLimit;
    
    public StorageRouter(FileStorage fileStorage, MemoryStorage memoryStorage, long switchLimit = 2_500_000)
    {
        _fileStorage = fileStorage;
        _memoryStorage = memoryStorage;
        _switchLimit = switchLimit;
        
        _fileStorage.LoadExisting();
    }
    
    public StorageRouter(IOptions<DBConfigSchema> config, long switchLimit = 2_500_000) : this(
        new FileStorage(config.Value.Directory, config.Value.Limit),
        new MemoryStorage(config.Value.MemoryStorageLimit),
        switchLimit)
    {
    }

    public async Task<bool> AddObject(string id, byte[] data, bool shouldOverride)
    {
        IStorage storage = data.Length > _switchLimit ? _fileStorage : _memoryStorage;

        try
        {
            var result = await storage.AddObject(id, data);
            Console.WriteLine($"Added Object {id} [{data.Length}B] ({storage.GetType().Name})");
            return result;
        }
        catch (Exception _)
        {

            IStorage other = storage == _fileStorage ? _memoryStorage : _fileStorage;

            Console.WriteLine($"Added {id} [{data.Length}B] FAILED! Storing in opposite storage ({other.GetType().Name})");
            var result = await other.AddObject(id, data);
            Console.WriteLine($"Added Object {id} [{data.Length}B]");

            return result;
        }
    }

    public async Task<bool> DeleteObject(string id)
    {
        var storage = await GetStorageById(id);
        return await storage.DeleteObject(id);
    }

    public async Task<bool> ObjectExists(string id)
    {
        var storagesSearches = new List<Task<bool>>
        {
            _fileStorage.ObjectExists(id),
            _memoryStorage.ObjectExists(id)
        };

        while (storagesSearches.Any())
        {
            var hasInStorageTask = await Task.WhenAny(storagesSearches);
            
            if (await hasInStorageTask) return true;
            
            storagesSearches.Remove(hasInStorageTask);
        }

        return false;
    }

    public async Task<byte[]?> GetObject(string id)
    {
        var storage = await GetStorageById(id);
        return await storage.GetObject(id);
    }

    public void Dispose()
    {
        _fileStorage.Dispose();
        _memoryStorage.Dispose();
    }

    private async Task<IStorage> GetStorageById(string id)
    {
        var isMemoryStored = _memoryStorage.ObjectExists(id);
        var isFileStored = _fileStorage.ObjectExists(id);
        var searches = new List<Task<bool>>
        {
            isFileStored,
            isMemoryStored
        };

        while (searches.Any())
        {
            var first = await Task.WhenAny(searches);
            searches.Remove(first);
            var result = await first;
            
            if (result)
            {
                return first == isMemoryStored ? _memoryStorage : _fileStorage;
            }
        }
        
        throw new Exception($"No storage found with id {id}");
    }
}