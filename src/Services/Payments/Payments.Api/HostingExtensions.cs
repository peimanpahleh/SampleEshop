using Consul;
using Payments.Application;
using Payments.Application.IntegrationEventsHandler;
using Payments.Infrastructure;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Payments.Api;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var settings = GetServiceSettings(builder.Configuration);

        builder.Services.AddScoped<IEventBus, MassTransitEventBus>();

        builder.Services
            .AddConsulClient(settings)
            .AddCustomMassTransit(settings)
            .AddCustomCors();

        // add layers
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(settings);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("CorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () =>
        {
            return "Welcome to payments";
        });

        return app;
    }

    private static ServiceSettings GetServiceSettings(IConfiguration configuration)
    {
        // check consul
        var consulCfg = configuration.GetValue<bool>("ConsulCfg");
        ServiceSettings settings = new();
        if (consulCfg == true)
        {
            var consulAddress = configuration["ServiceSettings:Consul"];
            var consulKey = configuration["ConsulKey"];
            var client = new ConsulClient(cfg => cfg.Address = new Uri(consulAddress));
            var res = client.KV.Get(consulKey).GetAwaiter().GetResult();

            if (res.Response == null || res.StatusCode == HttpStatusCode.NotFound)
                throw new InvalidOperationException("Consul config is null");

            var strResponse = Encoding.UTF8.GetString(res.Response.Value);

            settings = JsonSerializer.Deserialize<ServiceSettings>(strResponse);
        }
        else
        {
            settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        }

        return settings;
    }

    private static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });
        return services;
    }

    private static IServiceCollection AddConsulClient(this IServiceCollection services, ServiceSettings settings)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p =>
            new ConsulClient(consulConfig =>
            {
                var address = settings.Consul;
                consulConfig.Address = new Uri(address);
            }));

        return services;
    }

    private static IServiceCollection AddCustomMassTransit(this IServiceCollection services, ServiceSettings settings)
    {

        var mtSettings = settings.MassTransitSettings;

        if (mtSettings?.RabbitmqHost == null)
            throw new InvalidOperationException("MassTransitSettings is null");


        services.AddMassTransit(x =>
        {
            x.ApplyCustomMassTransitConfiguration();

            x.AddConsumersFromNamespaceContaining<OrderStatusChangedToStockConfirmedHandler>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(mtSettings.RabbitmqHost, mtSettings.RabbitmqVirtualHost, h =>
                {
                    h.Username(mtSettings.RabbitmqUsername);
                    h.Password(mtSettings.RabbitmqPassword);
                });

                cfg.ApplyCustomBusConfiguration();

                cfg.PrefetchCount = 20;

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService(true);


        return services;
    }


}
