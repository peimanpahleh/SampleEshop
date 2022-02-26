
namespace Products.Api.Controllers.User;

[Route("api/[controller]")]
public class ImagesController : BaseController
{
    private readonly IMediator _mediator;

    public ImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var query = new UserGetImageQuery(id);
        var res = await _mediator.Send(query);

        if (res == null)
            return NotFound();

        MemoryStream memory = new MemoryStream(res.Bytes);
        
        memory.Position = 0;

        // file name : for example testimg
        var type = res.ContentType.Split('/');

        var name = res.Id + "." + type[1];

        return File(memory, res.ContentType, name);
    }

}
