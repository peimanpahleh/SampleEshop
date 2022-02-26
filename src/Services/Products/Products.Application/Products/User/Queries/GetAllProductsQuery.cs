
namespace Products.Application.Products.User.Queries;

public record GetAllProductsQuery(int PageIndex, int PageSize) : IQuery<PagedList<ProductListDto>>;


public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, PagedList<ProductListDto>>
{
    private readonly IReadProductRepository _productRepo;

    public GetAllProductsQueryHandler(IReadProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<PagedList<ProductListDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var res = await _productRepo.GetAllProduct(request.PageIndex, request.PageSize, cancellationToken);


        var mapper = new PagedList<ProductListDto>()
        {
            TotalItems = res.TotalItems,
            TotalPages = res.TotalPages,
            Result = res.Result.Select(x => new ProductListDto(x.Id, x.Name,x.Quantity, x.Price,x.ImageId)).ToList()
        };

        return mapper;
    }
}
