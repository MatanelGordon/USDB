namespace Common.Compression.Abstraction;

public interface ICompression
{
    public string Extension { get; }
    
    Task<byte[]> Compress(byte[] data);

    Task<byte[]> Decompress(byte[] data);
}
