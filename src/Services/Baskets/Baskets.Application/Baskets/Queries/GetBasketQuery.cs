

using Baskets.Application.Models.Dto.User;

namespace Baskets.Application.Baskets.Queries;

public record GetBasketQuery(string CustomerId) : IQuery<BasketDto>;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, BasketDto>
{
    private readonly IBasketRepository _basketRepository;

    public GetBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<BasketDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var res = await _basketRepository.GetBasketAsync(Guid.Parse(request.CustomerId));

        if (res == null)
            return null;

        var basketDto = new BasketDto(res.Id.ToString(),
            res.BuyerName,
            res.BasketItems
            .Select(x => new BasketItemDto(x.Id.ToString(), x.ProductId, x.ProductName, x.UnitPrice,x.UnitPriceId, x.Quantity,x.ImageId)).ToList());

        return basketDto;

    }
}
