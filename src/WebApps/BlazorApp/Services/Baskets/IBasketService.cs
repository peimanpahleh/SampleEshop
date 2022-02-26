namespace BlazorApp.Services.Baskets;

public interface IBasketService
{
    Task<BasketDto> GetBasketAsync();
    Task<ResponseResult<string>> UpdateBasketAsync(UpdateBasketModel basket);
    Task<bool> DeleteBasketItemAsync(string id);
    Task<ResponseResult<bool>> CheckoutBasketAsync();
}
