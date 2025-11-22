using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;

namespace Recchx.Users.Application.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, string IpAddress, string UserAgent) 
    : IRequest<Result<AuthResponseDto>>;
