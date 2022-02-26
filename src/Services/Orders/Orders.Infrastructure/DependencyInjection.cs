namespace Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ServiceSettings settings)
    {
        BsonClassMap.RegisterClassMap<OrderItem>(cm =>
        {
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.AutoMap();
        });
      
        var mongo = settings.OrderSettings.MongoStoreDatabase;

        if (settings.MongoDbConnection == null || mongo.DatabaseName == null)
            throw new InvalidOperationException("mongo is null");

        var mongoClient = new MongoClient(settings.MongoDbConnection);
        var mongoDatabase = mongoClient.GetDatabase(mongo.DatabaseName);
        services.AddSingleton(mongoDatabase);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOrderRepository, MongoOrderRepository>();
        services.AddScoped<IBuyerRepository, MongoBuyerRepository>();
        services.AddScoped<IReadOrderRepository, MongoReadOrderRepository>();

        return services;
    }
}
