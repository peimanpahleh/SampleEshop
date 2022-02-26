namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ServiceSettings settings)
    {      
        // Add services to the container.
        BsonClassMap.RegisterClassMap<Product>(cm =>
        {
            cm.AutoMap();
            cm.MapField("_prices").SetElementName("Prices");
        });     
       
        if (settings.MongoDbConnection == null || settings.ProductSettings.MongoStoreDatabase.DatabaseName == null)
            throw new InvalidOperationException("mongo is null");

        var mongoClient = new MongoClient(settings.MongoDbConnection);
        var mongoDatabase = mongoClient.GetDatabase(settings.ProductSettings.MongoStoreDatabase.DatabaseName);
        services.AddSingleton(mongoDatabase);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // mongo repos
        services.AddScoped<IProductRepository, MongoProductRepository>();
        services.AddScoped<IReadProductRepository, MongoReadProductRepository>();
        services.AddScoped<IImageRepository, MongoImageRepository>();


        return services;
    }
}
