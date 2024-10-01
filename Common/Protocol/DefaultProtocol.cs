using System.IO.Hashing;
using Common.Protocol.Abstraction;
namespace Common.Protocol;

public class DefaultProtocol : IProtocol
{
    const int CRC32_SIZE_OVERHEAD = 4;

    public async Task<byte[]> Unwrap(Stream stream)
    {
        var len = ConstructLen(stream);
        var data = new byte[len];

        await stream.ReadExactlyAsync(data);

        var crcResult = new byte[CRC32_SIZE_OVERHEAD];
        await stream.ReadExactlyAsync(crcResult);

        var redoCRC = Crc32.Hash(data);

        var isMatched = crcResult.Zip(redoCRC).All(zip => (zip.First ^ zip.Second) == 0);

        if (!isMatched)
        {
            throw new Exception("Failed CRC32 Validation While Unwraping data");
        }

        return data;
    }

    public byte[] Wrap(byte[] data)
    {
        var crc32 = Crc32.Hash(data);

        if (crc32 is null)
        {
            throw new Exception("Failed to CRC32 Message");
        }

        var result = DeconstructLen(data.Length).ToList();

        result.AddRange(data);
        result.AddRange(crc32);

        return result.ToArray();
    }

    private byte[] DeconstructLen(int len) => BitConverter.GetBytes(len);

    private int ConstructLen(Stream stream)
    {
        var bytes = new byte[4].Select(_ => (byte) stream.ReadByte()).ToArray();
        return BitConverter.ToInt32(bytes);
    }
}