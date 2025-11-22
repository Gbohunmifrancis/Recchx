using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recchx.Users.Application.Commands.UpdateUserProfile;
using Recchx.Users.Application.Queries.GetUserProfile;
using System.Security.Claims;

namespace Recchx.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserProfileQuery { UserId = userId };
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
    {
        command.UserId = GetCurrentUserId();
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}

