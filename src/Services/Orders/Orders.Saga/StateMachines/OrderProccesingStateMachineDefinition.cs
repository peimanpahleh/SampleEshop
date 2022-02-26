namespace Orders.Saga.StateMachines;

public class OrderProccesingStateMachineDefinition : SagaDefinition<OrderState>
{
    private const int ConcurrencyLimit = 20;

    public OrderProccesingStateMachineDefinition()
    {
        ConcurrentMessageLimit = ConcurrencyLimit;

    }
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
    {

        endpointConfigurator.PrefetchCount = ConcurrencyLimit;

        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        endpointConfigurator.UseInMemoryOutbox();

        var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);

        sagaConfigurator.Message<OrderStarted>(x => x.UsePartitioner(partition, m => m.Message.OrderId));
        sagaConfigurator.Message<OrderStockConfirmed>(x => x.UsePartitioner(partition, m => m.Message.OrderId));
        sagaConfigurator.Message<OrderStockRejected>(x => x.UsePartitioner(partition, m => m.Message.OrderId));
        sagaConfigurator.Message<OrderPaymentSucceeded>(x => x.UsePartitioner(partition, m => m.Message.OrderId));
        sagaConfigurator.Message<OrderPaymentFailed>(x => x.UsePartitioner(partition, m => m.Message.OrderId));

    }
}
