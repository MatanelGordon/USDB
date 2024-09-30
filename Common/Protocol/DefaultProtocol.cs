using System.IO.Hashing;
using Common.Protocol.Abstraction;
namespace Common.Protocol;

public class DefaultProtocol : IProtocol
{
    const int CRC32_SIZE_OVERHEAD = 4;

    public async Task<byte[]> Unwrap(Stream stream)
    {
        var lenMSB = stream.ReadByte();
        var lenLSB = stream.ReadByte();
        var len = lenMSB * 256 + lenLSB;
        var data = new byte[len];

        await stream.ReadAsync(data);

        var crcResult = new byte[CRC32_SIZE_OVERHEAD];
        await stream.ReadAsync(crcResult);

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
        var len = data.Length;

        if (len > 256 * 256)
        {
            throw new Exception("Message Too Big");
        }

        var crc32 = Crc32.Hash(data);

        if (crc32 is null)
        {
            throw new Exception("Failed to CRC32 Message");
        }

        var lenLSB = (byte)(data.Length % 256);
        var lenMSB = (byte)(data.Length >> 8);
        var result = new List<byte>() { lenMSB, lenLSB };

        result.AddRange(data);
        result.AddRange(crc32);

        return result.ToArray();
    }
}
