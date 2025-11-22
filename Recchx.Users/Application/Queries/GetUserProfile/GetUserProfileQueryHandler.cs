using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public GetUserProfileQueryHandler(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (profile == null)
        {
            return Result.Failure<UserProfileDto>("User profile not found");
        }

        var result = new UserProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Summary = profile.Summary,
            Skills = profile.Skills,
            Preferences = profile.Preferences,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };

        return Result.Success(result);
    }
}

