namespace BlazorApp.Models.Products.Admin;

// get by id
public record AdminProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public long Price { get; set; }
    public string ImageId { get; set; }
    public IEnumerable<ProductPrice> Prices { get; set; }


    public AdminProductDto()
    {

    }

    public AdminProductDto(string id, string name, int quantity, long price, string imageId, IEnumerable<ProductPrice> prices)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        ImageId = imageId;
        Prices = prices;
    }
}
