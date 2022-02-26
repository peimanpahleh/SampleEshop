
namespace Products.Application.Models.Dto.Admin;

public record AdminProductDto(string Id, string Name, int Quantity, long Price, string ImageId, IEnumerable<ProductPrice> Prices);

