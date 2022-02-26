namespace Products.Domain.Images;

public class Image : Entity , IAggregateRoot
{
    public byte[] Bytes { get; private set; }
    public string ContentType { get; private set; }
    public long Length { get; private set; }

    public Image(byte[] bytes, string contentType, long length)
    {
        Bytes = bytes;
        ContentType = contentType;
        Length = length;
    }
}
