namespace Orders.Application.Models.Dto.User;

public record UserAllOrdersDto(string OrderId,decimal TotalPrice,int TotalItems, string Status);

