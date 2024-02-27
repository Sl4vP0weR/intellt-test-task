namespace API.Controllers;

[ApiController]
[Route("users/")]
public class UserController(
    IMapper Mapper,
    IMediator Mediator
) : ControllerBase
{   
    // We can use different types for DTO, but now we'll stick with domain requests, because for now they fit as DTO
    
    [HttpPost("/assign")]
    public async Task<IActionResult> Assign(AssignUser request, CancellationToken token)
    {
        await Mediator.Send(request, token);
        return Ok();
    }
    
    [HttpPost("/query")]
    public async Task<IActionResult> Query(QueryOrganizationUsers request, CancellationToken token)
    {
        var query = await Mediator.Send(request, token);
        return Ok(query);
    }
}