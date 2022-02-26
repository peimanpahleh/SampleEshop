namespace BlazorApp.Models.Orders.User;

public record UserAllOrdersDto(string OrderId, decimal TotalPrice, int TotalItems, string Status);

