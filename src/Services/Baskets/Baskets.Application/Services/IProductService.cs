using Baskets.Application.Models.Dto.User;

namespace Baskets.Application.Services;

public interface IProductService
{
    Task<IEnumerable<BasketItem>> GetProductByIds(IEnumerable<BasketUpdateItemDto> basketItemDto);

}
