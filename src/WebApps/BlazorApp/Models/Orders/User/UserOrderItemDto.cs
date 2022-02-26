namespace BlazorApp.Models.Orders.User;

public record UserOrderItemDto(string Id, string ProductId, string ProductName, decimal Price, string ImageId);

