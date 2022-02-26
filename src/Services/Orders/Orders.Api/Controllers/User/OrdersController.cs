namespace Orders.Api.Controllers.User;

[Route("api/[controller]")]
public class OrdersController : BaseController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int pageIndex, int pageSize)
    {
        var query = new UserGetAllOrdersQuery(pageIndex, pageSize);

        var res = await _mediator.Send(query);

        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByOrderId(Guid id)
    {
        var query = new UserGetOrderQuery(id.ToString());

        var res = await _mediator.Send(query);

        return Ok(res);
    }

}
