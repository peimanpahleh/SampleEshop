namespace BlazorApp.Services.Orders;

public interface IOrderService
{
    Task<PagedList<AdminAllOrdersDto>> AdminGetAllAsync(PageParameter parameter, string BuyerId);

    Task<AdminOrderDto> AdminGetByIdAsync(string id);


    Task<PagedList<UserAllOrdersDto>> UserGetAllAsync(PageParameter parameter, string BuyerId);

    Task<UserOrderDto> UserGetByIdAsync(string id);

}


