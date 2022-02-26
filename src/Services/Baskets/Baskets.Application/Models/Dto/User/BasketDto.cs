namespace Baskets.Application.Models.Dto.User;

public record BasketDto(string BuyerId, string BuyerName, List<BasketItemDto> BasketItems);

