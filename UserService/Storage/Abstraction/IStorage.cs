namespace UserService.Storage.Abstraction;

public interface IStorage: IDisposable
{
    Task<bool> AddObject(string id, byte[] data, bool shouldOverride = false);
    Task<byte[]?> GetObject(string id);
    Task<bool> DeleteObject(string id);
}
