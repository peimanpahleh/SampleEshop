
namespace Baskets.Api.Controllers.User;


[Route("api/[controller]")]
public class BasketsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identity;

    public BasketsController(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identity = identityService;
    }

    [HttpGet]
    public async Task<ActionResult> GetBasketByIdAsync()
    {
        var userId = _identity.GetUserIdentity();

        if (userId == null)
            return Unauthorized();

        var query = new GetBasketQuery(userId);
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UpdateBasketModel basket)
    {
        var userId = _identity.GetUserIdentity();
        var userName = _identity.GetUserName();

        if (userId == null || userName == null)
            return Unauthorized();

        var items = basket.BasketItems.Select(x => new BasketUpdateItemDto(x.ProductId, x.Quantity)).ToList();

        var basketDto = new BasketUpdateDto(userId, userName, items);

        var command = new UpdateBasketCommand(basketDto);
        var res = await _mediator.Send(command);

        if (res == null)
            return BadRequest(new FailedResult(400, "invalid product"));

        return Ok(new SuccessedResult(200, "success"));
    }

    [Route("checkout")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CheckoutAsync()
    {
        var userId = _identity.GetUserIdentity();

        var command = new CheckoutCommand(userId);
        var res = await _mediator.Send(command);

        if (!res)
            return BadRequest(new FailedResult(400, "can not checkout basket"));

        return Ok(new SuccessedResult(200, "your basket checkedout"));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBasketByIdAsync(string id)
    {
        var query = new DeleteBasketCommand(id);
        var res = await _mediator.Send(query);
        return Ok(res);
    }


    [HttpDelete("item/{id}")]
    public async Task<IActionResult> DeleteItemBasketAsync(string id)
    {
        var userId = _identity.GetUserIdentity();

        if (userId == null)
            return Unauthorized();

        var command = new DeleteBasketItemCommand(userId, id);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

}
