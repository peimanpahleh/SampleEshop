namespace Baskets.Application.Models.Dto.User;

public record BasketUpdateDto(
        string BuyerId,
        string BuyerName,
        List<BasketUpdateItemDto> BasketItems);

public record BasketUpdateItemDto(
        string ProductId,
        int Quantity);


// validators

public class BasketUpdateDtoValidator : AbstractValidator<BasketUpdateDto>
{
    public BasketUpdateDtoValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotNull();

        RuleFor(x => x.BuyerName)
            .NotNull();

        When(x => x.BuyerId != null, () =>
         {
             RuleFor(i => i.BuyerId.Length).GreaterThan(5);
         });

        When(x => x.BuyerName != null, () =>
        {
            RuleFor(i => i.BuyerName.Length).GreaterThan(2);
        });

        RuleFor(x => x.BasketItems.Count).GreaterThan(0);

        RuleForEach(x => x.BasketItems).SetValidator(new BasketUpdateItemValidator());

    }
}

public class BasketUpdateItemValidator : AbstractValidator<BasketUpdateItemDto>
{
    public BasketUpdateItemValidator()
    {
        RuleFor(x => x.ProductId.Length)
            .GreaterThan(2).WithMessage("ProductId is required");

        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be GreaterThan 0");
    }
}