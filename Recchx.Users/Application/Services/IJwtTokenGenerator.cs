using Recchx.Users.Domain.Entities;

namespace Recchx.Users.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, string sessionId, string jwtId, string deviceFingerprint);
    string GenerateRefreshToken();
}

