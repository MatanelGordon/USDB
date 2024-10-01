using Microsoft.Extensions.Options;
using UserService.Config;
using UserService.Storage.Abstraction;

namespace UserService.Storage;

internal class StorageRouter(FileStorage fileStorage, MemoryStorage memoryStorage, long switchLimit = 2_500_000)
    : IStorage
{
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
            return storage.AddObject(id, data);
        }
        catch (Exception _)
        {
            IStorage other = storage == fileStorage ? memoryStorage : fileStorage;

            return other.AddObject(id, data);
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
            fileStorage.ObjectExists(id),
            memoryStorage.ObjectExists(id)
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
        fileStorage.Dispose();
        memoryStorage.Dispose();
    }

    private async Task<IStorage> GetStorageById(string id)
    {
        var isMemoryStored = memoryStorage.ObjectExists(id);
        var isFileStored = fileStorage.ObjectExists(id);
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
                return first == isMemoryStored ? memoryStorage : fileStorage;
            }
        }
        
        throw new Exception($"No storage found with id {id}");
    }
}