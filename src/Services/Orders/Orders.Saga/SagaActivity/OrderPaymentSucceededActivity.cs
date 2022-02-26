namespace Orders.Saga.SagaActivity;

public class OrderPaymentSucceededActivity : Activity<OrderState>
{
    private readonly ILogger<OrderPaymentSucceededActivity> _logger;

    public OrderPaymentSucceededActivity(ILogger<OrderPaymentSucceededActivity> logger)
    {
        _logger = logger;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState> context, Behavior<OrderState> next)
    {
        await Execute(context);

        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<OrderState, T> context, Behavior<OrderState, T> next)
    {
        await Execute(context);

        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, TException> context, Behavior<OrderState> next) where TException : Exception
    {
        return next.Faulted(context);

    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<OrderState, T, TException> context, Behavior<OrderState, T> next) where TException : Exception
    {
        return next.Faulted(context);

    }

    public void Probe(ProbeContext context)
    {
        var scopeName = "OrderPaymentSucceededActivity".ToLower();
        context.CreateScope(scopeName);
    }

    private async Task Execute(BehaviorContext<OrderState> context)
    {
        var items = context.Instance.OrderItems
            .Select(i => new OrderStockItem(i.ProductId, i.Quantity)).ToList();

        var consumeContext = context.GetPayload<ConsumeContext>();

        await consumeContext.Publish(new OrderStatusChangedToPaid(
            context.Instance.OrderId.ToString(),
            context.Instance.BuyerId,
            context.Instance.BuyerName,
            items));

        _logger.LogInformation($"OrderPaymentSucceededActivity published OrderStatusChangedToPaid");
    }
}
