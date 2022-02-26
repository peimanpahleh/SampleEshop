
namespace Products.Application.Products.User.Queries;

public record GetProductByIdsQuery(string Ids) : IQuery<List<ProductGrpcDto>>;

public class GetProductByIdsQueryHandler : IQueryHandler<GetProductByIdsQuery, List<ProductGrpcDto>>
{
    private readonly IReadProductRepository _productRepo;

    public GetProductByIdsQueryHandler(IReadProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<List<ProductGrpcDto>> Handle(GetProductByIdsQuery request, CancellationToken cancellationToken)
    {
        if (request.Ids == null)
            return null;

        var ids = request.Ids.Split(',').ToList();
        var res = await _productRepo.GetByIdsAsync(ids, cancellationToken);

        // map product to productGrpcDto
        var items = res
            .Select(p => new ProductGrpcDto(p.Id, p.Name, p.Quantity, p.Price, p.PriceId, p.ImageId))
            .ToList();

        return items;
    }
}