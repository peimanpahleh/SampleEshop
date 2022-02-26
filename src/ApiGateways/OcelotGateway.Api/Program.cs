using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using OcelotGateway.Api.Configuration;
using OcelotGateway.Api.Extensions;
using Serilog;
using Serilog.Enrichers.Span;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Load options from appsettings.json
    builder.Services.Configure<OcelotSwaggerOptions>(builder.Configuration.GetSection(nameof(OcelotSwaggerOptions)));

    builder.Configuration.AddJsonFile("ocelot.json");
    builder.Configuration.AddJsonFile("ocelotswagger.json");

    builder.Services.AddOcelotSwagger();
    builder.Services.AddOcelot(builder.Configuration).AddConsul();

    builder.Services.AddControllers();


    builder.Host.UseSerilog((context, cfg) =>
    {
        cfg.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}");
        cfg.Enrich.FromLogContext();
        cfg.Enrich.WithSpan();
        cfg.ReadFrom.Configuration(context.Configuration);
    });

    var app = builder.Build();


    app.UseAuthorization();

    app.UseStaticFiles();


    app.UseOcelotSwagger();

    app
    .UseOcelot()
    .Wait();

    app.MapControllers();


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


