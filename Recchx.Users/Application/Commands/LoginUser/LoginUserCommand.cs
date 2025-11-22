using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;

namespace Recchx.Users.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

