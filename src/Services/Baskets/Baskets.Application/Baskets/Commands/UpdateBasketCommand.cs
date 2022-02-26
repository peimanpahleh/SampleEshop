
namespace Baskets.Application.Baskets.Commands;

public record UpdateBasketCommand(BasketUpdateDto Basket) : ICommand<string>;

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
         RuleFor(x => x.Basket).SetValidator(new BasketUpdateDtoValidator());
    }
}

public class UpdateBasketCommandHandler : ICommandHandler<UpdateBasketCommand, string>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductService _productService;

    public UpdateBasketCommandHandler(IBasketRepository basketRepository, IProductService productService)
    {
        _basketRepository = basketRepository;
        _productService = productService;
    }

    public async Task<string> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {

        var basketItems = await _productService.GetProductByIds(request.Basket.BasketItems);

        if (basketItems == null) 
            return null;

        // check for exist basket
        var findBasket = await _basketRepository.GetBasketAsync(Guid.Parse(request.Basket.BuyerId));

        if (findBasket != null)
        {
            var updatedBasket =  UpdateBasket(findBasket,basketItems);
            var resUpdate = await _basketRepository.UpdateBasketAsync(updatedBasket);

            return resUpdate;
        }

        var basket = new Basket(Guid.Parse(request.Basket.BuyerId), request.Basket.BuyerName);
        basket.AddItems(basketItems.ToList());


        var res = await _basketRepository.UpdateBasketAsync(basket);

        return res;
    }

    private Basket UpdateBasket(Basket firstBasket, IEnumerable<BasketItem> updateBasketItems)
    {
        List<BasketItem> itemsShouldBeAdd = new();
        // check exist items
        foreach (var item in updateBasketItems)
        {
            var existItem = firstBasket.BasketItems.FirstOrDefault(b => b.ProductId == item.ProductId);
            if (existItem != null)
            {
                if (existItem.Quantity != item.Quantity)
                {
                    // add items
                    firstBasket.ChangeQuantity(existItem.Id, item.Quantity);
                }
            }
            else
            {
                // add new
                itemsShouldBeAdd.Add(item);
            }
        }

        if (itemsShouldBeAdd.Count > 0)
        {
            firstBasket.AddItems(itemsShouldBeAdd);
        }

        return firstBasket;
    }
}


