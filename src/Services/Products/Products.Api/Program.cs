Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog((context, cfg) =>
    {
        cfg.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}");
        cfg.Enrich.FromLogContext();
        cfg.Enrich.WithSpan();
        cfg.ReadFrom.Configuration(context.Configuration);
    });


    builder.WebHost.ConfigureAppConfiguration((hostingContext, b) =>
    {
        var consulCfg = builder.Configuration.GetValue<bool>("ConsulCfg");
        if (consulCfg)
        {
            var consulKey = builder.Configuration["ConsulKey"];

            b.AddConsul(consulKey, options =>
            {
                var consulAddress = builder.Configuration["ServiceSettings:Consul"];

                options.ConsulConfigurationOptions = cco =>
                {
                    cco.Address = new Uri(consulAddress);
                };

                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext =>
                {
                    exceptionContext.Ignore = true;
                };
            }).AddEnvironmentVariables();
        }
    });

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    if (args.Contains("/seed"))
    {
        Log.Information("Seeding database...");
        SeedData.Seed(app);
        Log.Information("Done seeding database. Exiting.");
        return;
    }

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
