using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;

namespace Recchx.Users.Application.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<Result<UserProfileDto>>
{
    public Guid UserId { get; set; }
    public string? Summary { get; set; }
    public List<string>? Skills { get; set; }
    public string? Preferences { get; set; }
}

