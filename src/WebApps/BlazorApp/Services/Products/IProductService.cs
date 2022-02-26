namespace BlazorApp.Services.Products;

public interface IProductService
{
    Task<PagedList<AdminProductListDto>> AdminGetAllAsync(PageParameter parameter);
    Task<AdminProductDto> AdminGetByIdAsync(string id);

    Task<ResponseResult<string>> AdminAddProduct(AdminAddProductDto product);

    Task<ResponseResult<string>> AdminAddImage(IBrowserFile file);


    Task<PagedList<ProductListDto>> GetAllAsync(PageParameter parameter);
    Task<ProductDto> GetByIdAsync(string id);


}
