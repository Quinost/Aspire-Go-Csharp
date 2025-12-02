using API.Infrastructure.Commands;
using API.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/job")]
[ApiController]
public class JobController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddJob([FromBody] AddJobRequest request)
    {
        var guid = await mediator.Send(request);
        return Ok(guid);
    }
}
