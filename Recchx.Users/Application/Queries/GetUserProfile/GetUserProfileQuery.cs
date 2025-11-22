using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;

namespace Recchx.Users.Application.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<Result<UserProfileDto>>
{
    public Guid UserId { get; set; }
}

