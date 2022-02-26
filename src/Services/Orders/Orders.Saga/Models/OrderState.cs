namespace Orders.Saga.Models;

public class OrderState : SagaStateMachineInstance, ISagaVersion
{
    [BsonId]
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }



    public Guid OrderId { get; set; }
    public string BuyerId { get; set; }
    public string BuyerName { get; set; }

    public IEnumerable<CustomerBasketItem> OrderItems { get; set; }

    public string Desc { get; set; }

    public Guid? GracePeriodToken { get; set; }
    public int Version { get; set; }
}
