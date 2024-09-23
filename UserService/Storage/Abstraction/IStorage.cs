namespace Localos.Storage.Abstraction;

internal interface IStorage: IDisposable
{
    Task<bool> AddObject(string id, byte[] data, bool shouldOverride = false);
    Task<byte[]?> GetObject(string id);
    Task<bool> DeleteObject(string id);
}
