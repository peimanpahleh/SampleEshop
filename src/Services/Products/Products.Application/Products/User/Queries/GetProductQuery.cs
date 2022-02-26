
namespace Products.Application.Products.User.Queries;

public record GetProductQuery(string Id) : IQuery<ProductDto>;

public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto>
{
    private readonly IReadProductRepository _productRepo;

    public GetProductQueryHandler(IReadProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            return null;

        var res = await _productRepo.GetProductById(request.Id);

        if (res == null)
            return null;

        var mapper = new ProductDto(res.Id,res.Name,res.Quantity, res.Price,res.ImageId);

        return mapper;
    }
}
