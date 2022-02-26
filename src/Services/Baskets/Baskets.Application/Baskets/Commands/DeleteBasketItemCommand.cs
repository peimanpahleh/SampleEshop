
namespace Baskets.Application.Baskets.Commands;

public record DeleteBasketItemCommand(string CustomerId, string ProductId) : ICommand<bool>;

public class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommand>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull();

        When(x => x.ProductId != null, () =>
        {
            RuleFor(i => i.ProductId.Length).GreaterThan(5);
        });
    }
}

public class DeleteBasketItemCommandHandler : ICommandHandler<DeleteBasketItemCommand, bool>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketItemCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }


    public async Task<bool> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _basketRepository.DeleteBasketItemAsync(Guid.Parse(request.CustomerId), Guid.Parse(request.ProductId));
        return res;
    }
}