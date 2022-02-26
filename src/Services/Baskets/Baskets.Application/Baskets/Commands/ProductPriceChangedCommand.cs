using Baskets.Application.Models.Dto.Admin;

namespace Baskets.Application.Baskets.Commands;

public record ProductPriceChangedCommand(ProductPriceChangedDto ProductPrice) : ICommand<bool>;

public class ProductPriceChangedCommandHandler : ICommandHandler<ProductPriceChangedCommand, bool>
{
    private readonly IBasketRepository _repository;

    public ProductPriceChangedCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ProductPriceChangedCommand request, CancellationToken cancellationToken)
    {
        var userIds = _repository.GetUsers();

        foreach (var id in userIds)
        {
            
            var basket = await _repository.GetBasketAsync(Guid.Parse(id));

            basket.ChangePrice(request.ProductPrice.ProductId, request.ProductPrice.NewPrice, request.ProductPrice.NewPriceId);
            
            await _repository.UpdateBasketAsync(basket);
        }

        return true;
    }
}
