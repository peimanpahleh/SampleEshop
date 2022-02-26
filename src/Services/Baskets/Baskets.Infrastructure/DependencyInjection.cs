using Baskets.Application.Services;
using Baskets.Infrastructure.Settings;

namespace Baskets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ServiceSettings settings)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddScoped<IProductService, ProductService>();


        if (string.IsNullOrEmpty(settings.BasketSettings.RedisConnection))
            throw new InvalidOperationException("redis connection is null");

        services.AddSingleton(sp =>
        {
            var configuration = ConfigurationOptions.Parse(settings.BasketSettings.RedisConnection, true);

            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        #region Add Grpc

        //if (string.IsNullOrEmpty(productGrpcUrl))
        //    throw new InvalidOperationException("GrpcUrls is null");

        //services.AddGrpcClient<MyProductServiceClient>(options =>
        //{
        //    options.Address = new Uri(productGrpcUrl);
        //})
        //.ConfigurePrimaryHttpMessageHandler(() =>
        //{
        //    var handler = new HttpClientHandler
        //    {
        //        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        //    };
        //    return handler;
        //});
        #endregion

        return services;
    }
}
