namespace Baskets.Application.Models.Dto.Admin;

public record ProductPriceChangedDto(
        string ProductId,
        decimal NewPrice,
        string NewPriceId);
