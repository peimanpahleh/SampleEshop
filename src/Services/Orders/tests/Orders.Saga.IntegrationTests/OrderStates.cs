namespace Orders.Saga.IntegrationTests;

public enum OrderStates
{

    None = 0,
    Initial = 1,
    Final = 2,
    Submitted = 3,
    AwaitingStockValidation = 4,
    Validated = 5,
    Paid = 6,
    Canceled = 7
}

