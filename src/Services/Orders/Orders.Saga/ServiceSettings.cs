namespace Orders.Saga;

public class ServiceSettings
{
    public string IdentityUrl { get; set; }
    public string Consul { get; set; }
    public string MongoDbConnection { get; set; }
    public MassTransitSettings MassTransitSettings { get; set; }
    public SagaSettings SagaSettings { get; set; }

}

public class SagaSettings
{
    public string SagaCollectionName { get; set; }
}
