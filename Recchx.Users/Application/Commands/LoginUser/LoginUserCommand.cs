using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;
using System.Text.Json.Serialization;

namespace Recchx.Users.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    // These are set by the controller from HttpContext, not sent by client
    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string UserAgent { get; set; } = string.Empty;
}

