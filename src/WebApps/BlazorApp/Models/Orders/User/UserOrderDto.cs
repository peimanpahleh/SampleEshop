namespace BlazorApp.Models.Orders.User;

public record UserOrderDto(string OrderId, string Status, IEnumerable<UserOrderItemDto> Items);

