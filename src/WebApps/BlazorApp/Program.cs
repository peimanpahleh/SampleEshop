
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    options.ProviderOptions.DefaultScopes.Add("roles");
    options.ProviderOptions.DefaultScopes.Add("product");
    options.ProviderOptions.DefaultScopes.Add("basket");
    options.ProviderOptions.DefaultScopes.Add("order");
});

builder.Services.AddApiAuthorization()
     .AddAccountClaimsPrincipalFactory<CustomUserFactory>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });



//var productUrl = "http://127.0.0.1:5010";

var gatewayUrl = "http://127.0.0.1:5020";

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services
    .AddHttpClient("ServiceApi", client => client.BaseAddress = new Uri(gatewayUrl))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services
    .AddHttpClient("ServiceApi.NoAuth", client => client.BaseAddress = new Uri(gatewayUrl));


builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ProductApi"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ProductApi.NoAuth"));



builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<IProductService, FakeProductService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBasketService, BasketService>();

//builder.Services.AddOptions();
//builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();
