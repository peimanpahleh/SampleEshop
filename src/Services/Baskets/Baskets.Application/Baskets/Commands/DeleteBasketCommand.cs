
namespace Baskets.Application.Baskets.Commands;

public record DeleteBasketCommand(string CustomerId) : ICommand<bool>;
public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotNull();

        When(x => x.CustomerId != null, () =>
        {
            RuleFor(i => i.CustomerId.Length).GreaterThan(5);
        });
    }
}

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var res = await _basketRepository.DeleteBasketAsync(Guid.Parse(request.CustomerId));
        return res;
    }
}
