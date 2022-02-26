namespace Products.Api.Services;

public class ProductService : MyProductService.MyProductServiceBase
{
    private readonly IMediator _mediator;

    public ProductService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ProductListResponse> GetProducts(ProductItemRequest request, ServerCallContext context)
    {
        var query = new GetProductByIdsQuery(request.Ids);
        var items = await _mediator.Send(query);


        // map ProductGrpcDto to ProductItemResponse
        var grpcItems = items.Select(p => new ProductItemResponse()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            PriceId = p.PriceId,
            Quantity = p.Quantity,
            ImageId = p.ImageId
        }).ToList();

        var res = new ProductListResponse();
        res.Product.AddRange(grpcItems);

        return res;
    }
}
