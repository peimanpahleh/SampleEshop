namespace Baskets.Application.Models.Dto.Admin;

public record AdminBasketItemDto(string Id,
        string ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity);
