namespace Baskets.Application.Models.Dto.User;


public record BasketItemDto(string Id,
        string ProductId,
        string ProductName,
        decimal UnitPrice,
        string UnitPriceId,
        int Quantity,
        string ImageId);