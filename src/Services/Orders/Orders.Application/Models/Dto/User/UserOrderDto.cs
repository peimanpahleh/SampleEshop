namespace Orders.Application.Models.Dto.User;

public record UserOrderDto(string OrderId, string Status,IEnumerable<UserOrderItemDto> Items);
public record UserOrderItemDto(string Id,string ProductId,string ProductName,decimal Price,string ImageId);

