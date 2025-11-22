using MediatR;
using Recchx.SharedKernel.Results;

namespace Recchx.Users.Application.Commands.Logout;

public record LogoutCommand(Guid UserId, string SessionId) : IRequest<Result<bool>>;
