using Baskets.Application.Models.Dto.User;
using Baskets.Application.Services;
using Grpc.Net.Client;
using Products.Api;
using static Products.Api.MyProductService;

namespace Baskets.Infrastructure.Services;

public class ProductService : IProductService
{
    //private readonly MyProductServiceClient _client;
    private readonly IConsulClient _consulClient;
    private readonly ILogger<ProductService> _logger;
    public ProductService(IConsulClient consulClient, ILogger<ProductService> logger)
    {
        //_client = client;
        _logger = logger;
        _consulClient = consulClient;
    }

    public async Task<IEnumerable<BasketItem>> GetProductByIds(IEnumerable<BasketUpdateItemDto> basketItemDto)
    {

        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var url = await GetRequestUriAsync("product-grpc",true);
        using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
        {
            HttpHandler = handler
        });

        var client = new MyProductServiceClient(channel);

        var req = new ProductItemRequest();
        var idArray = basketItemDto.Select(x => x.ProductId).ToArray();
        var ids = string.Join(",", idArray);
        req.Ids = ids;

        if (!req.Ids.Any())
            return null;

        List<BasketItem> basketItems = new();

        try
        {
            var res = await client.GetProductsAsync(req);

            // map ProductItemResponse
            if (res != null)
            {
                foreach (var item in basketItemDto)
                {
                    var product = res.Product.FirstOrDefault(x => x.Id == item.ProductId);
                    if (product != null)
                    {
                        // check for quantity 
                        if (item.Quantity <= product.Quantity)
                        {
                            var basketItem = new BasketItem(Guid.NewGuid(),
                                            product.Id,
                                            product.Name,
                                            product.Price,
                                            product.PriceId,
                                            item.Quantity,
                                            product.ImageId);

                            basketItems.Add(basketItem);
                        }

                    }
                }
            }

            if (basketItems.Count == 0)
                return null;

            return basketItems;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in get products via grpc ErrorMsg:{ex.Message}");
            return null;
        }


    }


    private async Task<Uri> GetRequestUriAsync(string serviceName, bool https)
    {
        //Get all services registered on Consul
        var allRegisteredServices = await _consulClient.Agent.Services();

        //Get all instance of the service went to send a request to
        var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

        //Get a random instance of the service
        var service = GetRandomInstance(registeredServices);

        if (service == null)
        {
            return null;
        }

        var uriBuilder = new UriBuilder();
        if (https)
        {
            uriBuilder.Scheme = "https";
            uriBuilder.Host = service.Address;
            uriBuilder.Port = service.Port;
        }
        else
        {
            uriBuilder.Scheme = "http";
            uriBuilder.Host = service.Address;
            uriBuilder.Port = service.Port;
        }

        return uriBuilder.Uri;
    }

    private AgentService GetRandomInstance(IList<AgentService> services)
    {
        Random _random = new();

        AgentService servToUse = services[_random.Next(0, services.Count)];

        return servToUse;
    }
}
