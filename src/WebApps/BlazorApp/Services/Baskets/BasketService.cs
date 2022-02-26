namespace BlazorApp.Services.Baskets;

public class BasketService : IBasketService
{
    private const string _basketUrl = "baskets/api/baskets";

    private readonly IHttpClientFactory _clientFactory;

    public BasketService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<BasketDto> GetBasketAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var response = await client.GetAsync(_basketUrl);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // handle this
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<BasketDto>(resStream, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                return model;
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new BasketDto();
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

    public async Task<ResponseResult<string>> UpdateBasketAsync(UpdateBasketModel basket)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var json = JsonSerializer.Serialize(basket);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_basketUrl, data);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var resModel = await JsonSerializer.DeserializeAsync<ResponseResult<string>>(resStream,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                return resModel;
            }
            else
            {
                try
                {
                    var errorModel = await response.Content.ReadFromJsonAsync<ResponseResult<string>>();

                    return new ResponseResult<string>(errorModel.Code, errorModel.Msg, null);
                }
                catch
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();

                    return new ResponseResult<string>((int)response.StatusCode, errorMsg, null);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            var msg = e.Message;

            if (e.InnerException != null)
                msg += $"\t InnerException:{e.InnerException.Message}";

            return new ResponseResult<string>(400, msg, null);

        }
    }

    public async Task<bool> DeleteBasketItemAsync(string id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var url = _basketUrl + $"/item/{id}";

            var response = await client.DeleteAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            var msg = e.Message;

            if (e.InnerException != null)
                msg += $"\t InnerException:{e.InnerException.Message}";

            return false;

        }
    }

    public async Task<ResponseResult<bool>> CheckoutBasketAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var url = _basketUrl + "/checkout";

            var response = await client.PostAsync(url, null);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var resModel = await JsonSerializer.DeserializeAsync<ResponseResult<bool>>(resStream,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                return resModel;
            }
            else
            {
                try
                {
                    var errorModel = await response.Content.ReadFromJsonAsync<ResponseResult<string>>();

                    return new ResponseResult<bool>(errorModel.Code, errorModel.Msg, false);
                }
                catch
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();

                    return new ResponseResult<bool>((int)response.StatusCode, errorMsg, false);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            var msg = e.Message;

            if (e.InnerException != null)
                msg += $"\t InnerException:{e.InnerException.Message}";

            return new ResponseResult<bool>(400, msg, false);

        }
    }
}
