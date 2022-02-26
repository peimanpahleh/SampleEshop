namespace BlazorApp.Services.Products;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string _productUrl = "products/api/products";
    private const string _productAdminUrl = "products/api/admin/products";
    private const string _productImageUrl = "products/api/admin/Images";

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<PagedList<AdminProductListDto>> AdminGetAllAsync(PageParameter parameter)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var url = $"{_productAdminUrl}?pageIndex={parameter.PageIndex}&pageSize={parameter.PageSize}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // handle this
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<PagedList<AdminProductListDto>>(resStream, new JsonSerializerOptions()
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

    public async Task<AdminProductDto> AdminGetByIdAsync(string id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var response = await client.GetAsync($"{_productAdminUrl}/{id}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<AdminProductDto>(resStream, new JsonSerializerOptions()
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

    public async Task<ResponseResult<string>> AdminAddProduct(AdminAddProductDto product)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi");

            var json = JsonSerializer.Serialize(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_productAdminUrl, data);

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

    public async Task<ResponseResult<string>> AdminAddImage(IBrowserFile file)
    {
        var client = _clientFactory.CreateClient("ServiceApi");

        //long maxFileSize = 1024 * 1024 * 15;
        // 15728640 = 15 mb
        // default 500 kb

        var readyToUpload = false;

        using var content = new MultipartFormDataContent();

        try
        {
            var fileContent = new StreamContent(file.OpenReadStream());

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(content: fileContent,
                        name: "file",
                        fileName: file.Name);
            readyToUpload = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        if (readyToUpload == false)
            return new ResponseResult<string>(400, "cannot upload!", null);

        try
        {
            var response = await client.PostAsync(_productImageUrl, content);

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
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            var msg = ex.Message;

            if (ex.InnerException != null)
                msg += $"\t InnerException:{ex.InnerException.Message}";

            return new ResponseResult<string>(400, msg, null);
        }
    }

    public async Task<PagedList<ProductListDto>> GetAllAsync(PageParameter parameter)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi.NoAuth");

            var url = $"{_productUrl}?pageIndex={parameter.PageIndex}&pageSize={parameter.PageSize}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // handle this
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<PagedList<ProductListDto>>(resStream, new JsonSerializerOptions()
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

    public async Task<ProductDto> GetByIdAsync(string id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ServiceApi.NoAuth");

            var response = await client.GetAsync($"{_productUrl}/{id}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer
                    .DeserializeAsync<ProductDto>(resStream, new JsonSerializerOptions()
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
}

