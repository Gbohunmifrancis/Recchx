using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recchx.Users.Application.Commands.LoginUser;
using Recchx.Users.Application.Commands.Logout;
using Recchx.Users.Application.Commands.RefreshToken;
using Recchx.Users.Application.Commands.RegisterUser;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Recchx.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterUserCommand command)
    {
        // Extract device information
        command.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        command.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        // Extract device information
        command.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        command.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var command = new RefreshTokenCommand(request.RefreshToken, ipAddress, userAgent);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var sessionIdClaim = User.FindFirst("session_id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(sessionIdClaim))
        {
            return BadRequest(new { error = "Invalid token claims" });
        }

        var command = new LogoutCommand(Guid.Parse(userIdClaim), sessionIdClaim);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllSessions()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return BadRequest(new { error = "Invalid token claims" });
        }

        var command = new LogoutAllSessionsCommand(Guid.Parse(userIdClaim));
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(new { message = "All sessions logged out successfully" });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = User.FindFirst(ClaimTypes.Surname)?.Value;
        var sessionId = User.FindFirst("session_id")?.Value;

        return Ok(new
        {
            userId,
            email,
            firstName,
            lastName,
            sessionId
        });
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

