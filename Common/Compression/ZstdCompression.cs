using Common.Compression.Abstraction;
using Microsoft.Extensions.Options;
using ZstdNet;

namespace Common.Compression;

public class ZstdCompression(IOptions<CompressionConfigSchema> compressionOptions) : ICompression
{
    public Task<byte[]> Compress(byte[] data)
    {
        using var zstdOptions = new CompressionOptions(compressionOptions.Value.Level);
        using var zstd = new Compressor(zstdOptions);
        return Task.FromResult(zstd.Wrap(data));
    }

    public Task<byte[]> Decompress(byte[] data)
    {
        using var zstd = new Decompressor();
        return Task.FromResult(zstd.Unwrap(data));
    }
}
