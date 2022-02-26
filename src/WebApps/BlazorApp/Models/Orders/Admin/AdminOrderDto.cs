namespace BlazorApp.Models.Orders.Admin;

public record AdminOrderDto(string OrderId, string BuyerId, string BuyerName, string Status,
    IEnumerable<AdminOrderItemDto> Items);
