
using Baskets.Application.Baskets.Commands;
using Baskets.Application.Models.Dto.Admin;

namespace Baskets.Application.IntegrationEvents.EventHandler;

public class ProductPriceChangedHandler : IIntegrationEventHandler<ProductPriceChanged>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductPriceChangedHandler> _logger;

    public ProductPriceChangedHandler(IMediator mediator,ILogger<ProductPriceChangedHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductPriceChanged> context)
    {
        var msg = context.Message;

        _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at Baskets.Application - ({@IntegrationEvent})", msg.Id, msg);

        var command = new  ProductPriceChangedCommand(new ProductPriceChangedDto(msg.ProductId,msg.NewPrice,msg.NewPriceId));

        await _mediator.Send(command);
    }
}
