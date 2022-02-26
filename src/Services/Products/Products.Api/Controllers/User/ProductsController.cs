
namespace Products.Api.Controllers.User;


[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex,[FromQuery] int pageSize)
    {
        var query = new GetAllProductsQuery(pageIndex,pageSize);
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var query = new GetProductQuery(id);
        var res = await _mediator.Send(query);
        return Ok(res);
    }
}
