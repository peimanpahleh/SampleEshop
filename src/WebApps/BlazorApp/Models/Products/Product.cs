namespace BlazorApp.Models.Products;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; } = 0;
    public int Quantity { get; set; } = 1;
    public string ImageId { get; set; }

    public Product()
    {

    }

    public Product(string id, string name, string imageId, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        ImageId = imageId;
        Quantity = quantity;
    }
}


