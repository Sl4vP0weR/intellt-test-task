namespace API.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IMediator Mediator) : ControllerBase
{   
    [HttpPost("/")]
    public async Task<IActionResult> Post(CreateUser request, CancellationToken token)
    {
        await Mediator.Send(request, token);
        return Ok();
    }
}