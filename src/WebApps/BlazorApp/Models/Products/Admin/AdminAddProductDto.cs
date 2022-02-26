namespace BlazorApp.Models.Products.Admin;

public class AdminAddProductDto
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ImageId { get; set; }

}

// (string Name, int Quantity, long Price, string ImageId)