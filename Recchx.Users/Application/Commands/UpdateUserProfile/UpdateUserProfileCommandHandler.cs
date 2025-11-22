using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;
using Recchx.Users.Domain.Entities;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<UserProfileDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public UpdateUserProfileCommandHandler(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository)
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<UserProfileDto>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserProfileDto>("User not found");
        }

        var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (profile == null)
        {
            // Create new profile
            profile = new UserProfile(request.UserId, request.Summary, request.Skills, request.Preferences);
            await _userProfileRepository.AddAsync(profile, cancellationToken);
        }
        else
        {
            // Update existing profile
            profile.UpdateProfile(request.Summary, request.Skills, request.Preferences);
            await _userProfileRepository.UpdateAsync(profile, cancellationToken);
        }

        await _userProfileRepository.SaveChangesAsync(cancellationToken);

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

