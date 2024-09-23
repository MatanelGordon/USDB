namespace Common.Protocol.Abstraction;

public interface IProtocol
{
    byte[] Wrap(byte[] data);

    Task<byte[]> Unwrap(Stream stream);
}
