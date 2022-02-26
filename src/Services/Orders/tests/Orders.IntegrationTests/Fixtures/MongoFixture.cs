using Orders.Application.DomainEventHandler;
using Orders.Infrastructure.Settings;

namespace Orders.IntegrationTests.Fixtures;

public class MongoFixture : IDisposable
{

    public ServiceProvider ServiceProvider { get; private set; }
    public IMongoDatabase MongoDatabase { get; private set; }

    private string _buyerCollectionName = "";
    private string _orderCollectionName = "";


    public MongoFixture()
    {
        AddDependency();
    }

    private void AddDependency()
    {
        /*BsonClassMap.RegisterClassMap<Entity>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
        });*/

        BsonClassMap.RegisterClassMap<OrderItem>(cm =>
        {
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.AutoMap();
        });


        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        // add mediator
        services.AddMediatR(typeof(UpdateOrderWhenBuyerVerifiedDomainEventHandler).Assembly);

        // Add mongo
        services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

        var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        var mongo = serviceSetting.OrderSettings.MongoStoreDatabase;
        if (mongo.DatabaseName == null)
            throw new InvalidOperationException("mongo is null");

        var mongoClient = new MongoClient(serviceSetting.MongoDbConnection);
        var mongoDatabase = mongoClient.GetDatabase(mongo.DatabaseName);
        services.AddSingleton(mongoDatabase);

        MongoDatabase = mongoDatabase;
        _buyerCollectionName = mongo.BuyerCollectionName;
        _orderCollectionName = mongo.OrderCollectionName;

        services.AddLogging();

        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEventBus, MassTransitEventBus>();
        services.AddScoped<IOrderRepository, MongoOrderRepository>();
        services.AddScoped<IBuyerRepository, MongoBuyerRepository>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void ResetState()
    {
        /*if (!string.IsNullOrEmpty(_buyerCollectionName))
            MongoDatabase.DropCollection(_buyerCollectionName);*/

        if (!string.IsNullOrEmpty(_orderCollectionName))
            MongoDatabase.DropCollection(_orderCollectionName);
    }


    public void Dispose()
    {
        // ResetState();
    }
}
