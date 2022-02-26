namespace Baskets.Domain.Baskets;

public interface IBasketRepository
{
    Task<Basket> GetBasketAsync(Guid customerId);
    IEnumerable<string> GetUsers();
    Task<string> UpdateBasketAsync(Basket basket);
    Task<bool> DeleteBasketAsync(Guid customerId);
    Task<bool> DeleteBasketItemAsync(Guid customerId, Guid productId);
}
