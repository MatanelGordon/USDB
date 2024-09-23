namespace Common.Compression.Abstraction;

public interface ICompression
{
    Task<byte[]> Compress(byte[] data);

    Task<byte[]> Decompress(byte[] data);
}
