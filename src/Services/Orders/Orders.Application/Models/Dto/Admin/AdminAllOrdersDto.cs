namespace Orders.Application.Models.Dto.Admin;

public record AdminAllOrdersDto(string OrderId, string BuyerId, string BuyerName, decimal TotalPrice, int TotalItems, string Status,
    DateTime CreatedAt);

