namespace Baskets.Application.Models.Dto.Admin;

public record AdminBasketDto(string BuyerId, List<AdminBasketItemDto> BasketItems);
