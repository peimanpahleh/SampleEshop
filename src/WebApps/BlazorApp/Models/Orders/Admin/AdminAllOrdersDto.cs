namespace BlazorApp.Models.Orders.Admin;

public record AdminAllOrdersDto(string OrderId, string BuyerId, string BuyerName, decimal TotalPrice, int TotalItems, string Status);
