
namespace Products.Application.Models.Dto.User;

public record ProductGrpcDto(string Id, string Name, int Quantity, long Price, string PriceId,string ImageId);
