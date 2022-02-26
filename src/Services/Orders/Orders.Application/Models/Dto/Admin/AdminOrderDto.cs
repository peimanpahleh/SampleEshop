namespace Orders.Application.Models.Dto.Admin;


public record AdminOrderDto(string OrderId,string BuyerId, string BuyerName, string Status, DateTime CreatedAt,
    IEnumerable<AdminOrderItemDto> Items);
public record AdminOrderItemDto(string Id, string ProductId, string ProductName, decimal Price,string ImageId);