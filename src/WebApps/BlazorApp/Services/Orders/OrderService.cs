namespace BlazorApp.Services.Orders;

public class OrderService : IOrderService
{
    private const string _orderUrl = "orders/api/orders";
    private const string _orderAdminUrl = "orders/api/admin/orders";

    private readonly IHttpClientFactory _clientFactory;

    public OrderService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<PagedList<AdminAllOrdersDto>> AdminGetAllAsync(PageParameter parameter, string buyerId)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var url = $"{_orderAdminUrl}?pageIndex={parameter.PageIndex}&pageSize={parameter.PageSize}";

            if (!string.IsNullOrEmpty(buyerId))
                url += $"&buyerId={buyerId}";

            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // handle this
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<PagedList<AdminAllOrdersDto>>(resStream, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                return model;
            }

            return null;
        }
        catch (Exception ex)
        {

            var msg = ex.Message;

            if (ex.InnerException != null)
            {
                msg = msg + $"/t InnerException:{ex.InnerException?.Message}";
            }

            Console.WriteLine(msg);

            return null;
        }
    }

    public async Task<AdminOrderDto> AdminGetByIdAsync(string id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var url = $"{_orderAdminUrl}/{id}";

            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<AdminOrderDto>(resStream, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                return model;
            }

            return null;
        }
        catch (Exception ex)
        {
            var msg = ex.Message;

            if (ex.InnerException != null)
            {
                msg = msg + $"/t InnerException:{ex.InnerException?.Message}";
            }

            Console.WriteLine(msg);

            return null;
        }
    }

    public Task<PagedList<UserAllOrdersDto>> UserGetAllAsync(PageParameter parameter, string BuyerId)
    {
        throw new NotImplementedException();
    }

    public Task<UserOrderDto> UserGetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}


