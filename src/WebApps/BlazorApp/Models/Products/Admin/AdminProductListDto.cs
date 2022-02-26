namespace BlazorApp.Models.Products.Admin;

// get all
public record AdminProductListDto(string Id, string Name, int Quantity, long Price, string ImageId);
