namespace Products.Domain.Products;

public class ProductPrice
{
    public string Id { get; private set; }
    public long Value { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ProductPrice(long value)
    {
        Id = Guid.NewGuid().ToString();
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }
}
