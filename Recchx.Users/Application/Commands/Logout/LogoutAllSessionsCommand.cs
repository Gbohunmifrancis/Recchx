using MediatR;
using Recchx.SharedKernel.Results;

namespace Recchx.Users.Application.Commands.Logout;

public record LogoutAllSessionsCommand(Guid UserId) : IRequest<Result<bool>>;
