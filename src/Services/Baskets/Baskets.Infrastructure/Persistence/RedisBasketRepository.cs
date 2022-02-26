
using Newtonsoft.Json;

namespace Baskets.Infrastructure.Persistence;

public class RedisBasketRepository : IBasketRepository
{
    private readonly ILogger<RedisBasketRepository> _logger;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisBasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
    {
        _logger = loggerFactory.CreateLogger<RedisBasketRepository>();
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<Basket> GetBasketAsync(Guid customerId)
    {
        var data = await _database.StringGetAsync(customerId.ToString());

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new BsketRedisJsonConverter());
        var res = JsonConvert.DeserializeObject<Basket>(data, settings);

        /*var res = JsonSerializer.Deserialize<Basket>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        });*/

        return res;
    }

    public IEnumerable<string> GetUsers()
    {
        var server = GetServer();
        var data = server.Keys();

        var res = data?.Select(k => k.ToString());

        return res;
    }

    public async Task<bool> DeleteBasketAsync(Guid customerId)
    {
        var res = await _database.KeyDeleteAsync(customerId.ToString());
        return res;
    }


    public async Task<string> UpdateBasketAsync(Basket basket)
    {
        // group items  
        // TODO check for new update
        var serialize = System.Text.Json.JsonSerializer.Serialize(basket);
        var created = await _database.StringSetAsync(basket.Id.ToString(), serialize);

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return null;
        }

        _logger.LogInformation("Basket item persisted succesfully.");

        return basket.Id.ToString();
    }

    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints();
        return _redis.GetServer(endpoint.First());
    }

    public async Task<bool> DeleteBasketItemAsync(Guid customerId, Guid productId)
    {
        // group items  
        // TODO check for new update

        var basket = await GetBasketAsync(customerId);

        if (basket == null)
            return false;

        //var product = basket.BasketItems.Any(x => x.ProductId == productId.ToString());

        //if (!product)
        //    return false;

        basket.RemoveItem(productId);

        var serialize = System.Text.Json.JsonSerializer.Serialize(basket);
        var created = await _database.StringSetAsync(basket.Id.ToString(), serialize);

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return false;
        }

        _logger.LogInformation("Basket item persisted succesfully.");

        return true;
    }
}
