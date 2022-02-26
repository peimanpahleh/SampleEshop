
using Payments.Api;

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


    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

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
