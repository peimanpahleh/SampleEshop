namespace Products.Domain.Products;

public class Product : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public string ImageId { get; private set; }

    private List<ProductPrice> _prices = new();
    public IReadOnlyList<ProductPrice> Prices => _prices.ToList();

    public long Price => Prices
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.Value)
                .FirstOrDefault();

    public string PriceId => Prices
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.Id)
                .FirstOrDefault();

    // for ef core
    protected Product()
    {

    }

    public Product(string name, int quantity, long price,string imageId)
    {
        Name = name;
        Quantity = quantity;
        ImageId = imageId;
        _prices.Add(new ProductPrice(price));
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangePrice(long price)
    {
        _prices.Add(new ProductPrice(price));
    }

    public void RemoveStock(int unit)
    {
        Quantity -= unit;
    }


}
