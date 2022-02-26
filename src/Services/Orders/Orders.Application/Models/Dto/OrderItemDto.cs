namespace Orders.Application.Models.Dto;

public record OrderItemDto(
    string Id,
    string ProductId,
    string ProductName,
    decimal UnitPrice,
    string UnitPriceId,
    int Quantity,
    string ImageId);
