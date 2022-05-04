
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.SystemConsole.Themes;
//using Serilog.Sinks.SystemConsole.Themes;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddScoped<IEventBus, MassTransitEventBus>();

        // check for consul
        ServiceSettings settings = new();

        var consulCfg = configuration.GetValue<bool>("ConsulCfg");
        if (consulCfg == true)
        {
            var consulAddress = configuration["ServiceSettings:Consul"];
            var client = new ConsulClient(cfg => cfg.Address = new Uri(consulAddress));
            var res = client.KV.Get("SampleEshop").GetAwaiter().GetResult();

            if (res.Response == null || res.StatusCode == HttpStatusCode.NotFound)
                throw new InvalidOperationException("Consul config is null");

            var strResponse = Encoding.UTF8.GetString(res.Response.Value);

            settings = JsonSerializer.Deserialize<ServiceSettings>(strResponse);
        }
        else
        {
            settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        }

        var mtSettings = settings.MassTransitSettings;

        if (mtSettings?.RabbitmqHost == null)
            throw new InvalidOperationException("MassTransitSettings is null");

        services.AddMassTransit(x =>
        {
            x.ApplyCustomMassTransitConfiguration();
            x.AddDelayedMessageScheduler();

            x.AddSagaStateMachine<OrderProccesingStateMachine, OrderState>
            (typeof(OrderProccesingStateMachineDefinition))
                .MongoDbRepository(r =>
                {
                    r.Connection = settings.MongoDbConnection;
                    r.DatabaseName = settings.SagaSettings.SagaCollectionName;
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(mtSettings.RabbitmqHost, mtSettings.RabbitmqVirtualHost, h =>
                {
                    h.Username(mtSettings.RabbitmqUsername);
                    h.Password(mtSettings.RabbitmqPassword);
                });

                cfg.ApplyCustomBusConfiguration();

                cfg.PrefetchCount = 20;
                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService(true);


        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .CreateLogger();

    }).UseSerilog()
    .Build();

await host.RunAsync();
