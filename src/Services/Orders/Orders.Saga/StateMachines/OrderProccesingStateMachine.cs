namespace Orders.Saga.StateMachines;

public class OrderProccesingStateMachine : MassTransitStateMachine<OrderState>
{

    public OrderProccesingStateMachine(ILogger<OrderProccesingStateMachine> logger)
    {

        InstanceState(x => x.CurrentState,
                        Submitted, // 3 
                        AwaitingStockValidation, // 4
                        Validated, // 5
                        Paid, // 6
                        Canceled //7
                        );

        SetUpEvents();

        SetUpSchedules();

        Initially(
           When(OrderSubmitted)
               .Then(context =>
               {
                   logger.LogInformation($"OrderProccesingStateMachine started orderId:{context.Data.OrderId}");
                   context.Instance.OrderId = Guid.Parse(context.Data.OrderId);
                   context.Instance.BuyerId = context.Data.BuyerId;
                   context.Instance.BuyerName = context.Data.BuyerName;
                   context.Instance.OrderItems = context.Data.OrderItems;
                   context.Instance.Desc = "Submitted";
               }) // copy data                   
               .Schedule(GracePeriodSchedule, context => context.Init<GracePeriodDone>(new GracePeriodDone()
               {
                   OrderId = Guid.Parse(context.Data.OrderId),
               }))
               .TransitionTo(Submitted));


        HandleSubmitedOrder();
        HandleAwaitingStockValidation();
        HandleValidated();
        HandlePaid();
        HandleCanceled();

    }


    private void SetUpEvents()
    {
        Event(() => OrderSubmitted, x => x.CorrelateById(context => Guid.Parse(context.Message.OrderId)));
        Event(() => OrderStockConfirmed, x => x.CorrelateById(context => Guid.Parse(context.Message.OrderId)));
        Event(() => OrderStockRejected, x => x.CorrelateById(context => Guid.Parse(context.Message.OrderId)));
        Event(() => OrderPaymentSucceeded, x => x.CorrelateById(context => Guid.Parse(context.Message.OrderId)));
        Event(() => OrderPaymentFailed, x => x.CorrelateById(context => Guid.Parse(context.Message.OrderId)));

    }

    private void SetUpSchedules()
    {

        Schedule(() => GracePeriodSchedule, instance => instance.GracePeriodToken, s =>
        {
            s.Delay = TimeSpan.FromSeconds(1);
            s.Received = r => r.CorrelateById(context => context.Message.OrderId);
        });

        Schedule(() => StockConfirmedSchedule, instance => instance.GracePeriodToken, s =>
        {
            s.Delay = TimeSpan.FromSeconds(1);
            s.Received = r => r.CorrelateById(context => context.Message.OrderId);
        });

        Schedule(() => StockRejectedSchedule, instance => instance.GracePeriodToken, s =>
        {
            s.Delay = TimeSpan.FromSeconds(1);
            s.Received = r => r.CorrelateById(context => context.Message.OrderId);
        });
    }


    private void HandleSubmitedOrder()
    {

        During(Submitted,
            When(GracePeriodSchedule?.Received)
            .TransitionTo(AwaitingStockValidation)
            .Activity(x => x.OfInstanceType<GracePeriodActivity>())
            .Then(context => context.Instance.Desc = "AwaitingStockValidation"));


        During(Submitted,
            Ignore(OrderSubmitted),
            Ignore(OrderStockConfirmed),
            Ignore(OrderStockRejected),
            Ignore(OrderPaymentSucceeded),
            Ignore(OrderPaymentFailed)
            );
    }

    private void HandleAwaitingStockValidation()
    {
        // OrderStockConfirmed

        During(AwaitingStockValidation,
            When(OrderStockConfirmed)
            .Schedule(StockConfirmedSchedule, context => context.Init<StockConfirmedReminder>(new StockConfirmedReminder()
            {
                OrderId = context.Instance.OrderId
            })));

        During(AwaitingStockValidation,
            When(StockConfirmedSchedule?.Received)
            .Then(c =>
            {
                c.Instance.Desc = "Validated";
            })
            .Activity(x => x.OfInstanceType<StockConfirmedActivity>())
            .TransitionTo(Validated));


        // OrderStockRejected

        During(AwaitingStockValidation,
            When(OrderStockRejected)
             .Schedule(StockRejectedSchedule, context => context.Init<StockRejectedReminder>(new StockRejectedReminder()
             {
                 OrderId = context.Instance.OrderId
             })));

        During(AwaitingStockValidation,
            When(StockRejectedSchedule?.Received)
            .Then(c =>
            {
                c.Instance.Desc = "Cancelled";
            })
            .Activity(x => x.OfInstanceType<StockConfirmedActivity>())
            .TransitionTo(Canceled));

        During(AwaitingStockValidation,
            Ignore(OrderSubmitted),
            Ignore(OrderPaymentSucceeded),
            Ignore(OrderPaymentFailed)
            );
    }

    private void HandleValidated()
    {
        During(Validated,
            When(OrderPaymentSucceeded)
             .Then(c =>
             {
                 c.Instance.Desc = "Paid";
             })
            .Activity(x => x.OfInstanceType<OrderPaymentSucceededActivity>())
            .TransitionTo(Paid));


        During(Validated,
            When(OrderPaymentFailed)
            .Then(c =>
            {
                c.Instance.Desc = "Cancelled";
            })
            .Activity(x => x.OfInstanceType<OrderPaymentFailedActivity>())
            .TransitionTo(Canceled));

        During(Validated,
            Ignore(OrderSubmitted),
            Ignore(OrderStockConfirmed),
            Ignore(OrderStockRejected)
            );
    }

    private void HandlePaid()
    {
        During(Paid,
            Ignore(OrderSubmitted),
            Ignore(OrderStockConfirmed),
            Ignore(OrderStockRejected),
            Ignore(OrderPaymentSucceeded),
            Ignore(OrderPaymentFailed)
            );
    }

    private void HandleCanceled()
    {
        During(Canceled,
            Ignore(OrderSubmitted),
            Ignore(OrderStockConfirmed),
            Ignore(OrderStockRejected),
            Ignore(OrderPaymentSucceeded),
            Ignore(OrderPaymentFailed)
            );
    }



    public State Submitted { get; private set; }
    public State AwaitingStockValidation { get; private set; }
    public State Validated { get; private set; }
    public State Paid { get; set; }
    public State Canceled { get; private set; }



    public Event<OrderStarted> OrderSubmitted { get; private set; }
    public Event<OrderStockConfirmed> OrderStockConfirmed { get; private set; }
    public Event<OrderStockRejected> OrderStockRejected { get; private set; }
    public Event<OrderPaymentSucceeded> OrderPaymentSucceeded { get; private set; }
    public Event<OrderPaymentFailed> OrderPaymentFailed { get; private set; }


    public Schedule<OrderState, GracePeriodDone> GracePeriodSchedule { get; private set; }
    public Schedule<OrderState, StockConfirmedReminder> StockConfirmedSchedule { get; private set; }
    public Schedule<OrderState, StockRejectedReminder> StockRejectedSchedule { get; private set; }
}
