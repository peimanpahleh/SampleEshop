using Consul;
using Microsoft.Extensions.Options;
using Orders.Infrastructure.Settings;
using System.Text;
using System.Text.Json;

namespace Orders.Api;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection(nameof(ServiceSettings)));

        var settings = GetServiceSettings(builder.Configuration);

        builder.Services.AddScoped<IEventBus, MassTransitEventBus>();

        builder.Services
           .AddConsulClient(settings)
           .AddCustomMassTransit(settings)
           .AddCustomCors()
           .AddCustomIdentity(settings);

        // add layers
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(settings);


        // Prevent mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder.Services.AddControllers();
        builder.Services.AddFluentValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
            fv.AutomaticValidationEnabled = false;
        });
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

        app.MapGet("/",() =>
        {
            return "Welcome to orders";
        });

        app.MapGet("/ping", () =>
        {
            return "pong";
        });

        app.UseConsul();

        return app;
    }

    private static void UseConsul(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ConsulClient>();

            var consulClient = scope.ServiceProvider.GetRequiredService<IConsulClient>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptions<ServiceSettings>>();

            var consulSettings = settings.Value.OrderSettings.ConsulSettings;

            var uniqueId = Guid.NewGuid().ToString();
            var serviceId = $"{consulSettings.Name}_{consulSettings.Address}:{consulSettings.Port}_{uniqueId}";
            var serviceGrpcId = $"{consulSettings.Name}_{consulSettings.Address}:{consulSettings.GrpcPort}_{uniqueId}";

            if (consulSettings.Port > 0)
            {

                var url = $"http://{consulSettings.Address}:{consulSettings.Port}/{consulSettings.Ping}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(15),
                    Interval = TimeSpan.FromSeconds(15),
                    HTTP = url
                };

                var registration = new AgentServiceRegistration()
                {
                    ID = serviceId,
                    Address = consulSettings.Address,
                    Name = consulSettings.Name,
                    Port = consulSettings.Port,
                    Checks = new[] { httpCheck }
                };

                var res = consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                logger.LogInformation($"http port registerd on port:{consulSettings.Port} and status:{res.StatusCode} time {res.RequestTime}");

            }

            if (consulSettings.GrpcPort > 0)
            {

                var url = $"http://{consulSettings.Address}:{consulSettings.Port}/{consulSettings.Ping}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(15),
                    Interval = TimeSpan.FromSeconds(15),
                    HTTP = url
                };

                var registration = new AgentServiceRegistration()
                {
                    ID = serviceGrpcId,
                    Address = consulSettings.Address,
                    Name = consulSettings.Name + "-grpc",
                    Port = consulSettings.GrpcPort,
                    Checks = new[] { httpCheck }
                };

                var res = consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                logger.LogInformation($"grpc port registerd on port:{consulSettings.GrpcPort} and status:{res.StatusCode} time {res.RequestTime}");

            }

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                logger.LogInformation("ApplicationStarted");
            });

            app.Lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                consulClient.Agent.ServiceDeregister(serviceGrpcId).GetAwaiter().GetResult();
                logger.LogInformation("ApplicationStopping");

            });

            app.Lifetime.ApplicationStopped.Register(() =>
            {
                logger.LogInformation("ApplicationStopping");

            });

        }
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

            x.AddConsumersFromNamespaceContaining<OrderStatusChangedToAwaitingValidationHandler>();

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


    private static IServiceCollection AddCustomIdentity(this IServiceCollection services, ServiceSettings settings)
    {
        var identityUrl = settings.IdentityUrl;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.Authority = identityUrl;
            options.Audience = "product-api";
            options.RequireHttpsMetadata = false;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "order");
            });
        });

        return services;
    }



}
