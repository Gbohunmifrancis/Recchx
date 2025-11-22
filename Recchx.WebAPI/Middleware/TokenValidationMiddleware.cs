using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.WebAPI.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserSessionRepository sessionRepository)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Extract JWT ID (jti) claim
                var jtiClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
                
                if (jtiClaim != null)
                {
                    var jwtId = jtiClaim.Value;
                    
                    // Check if session is still active
                    var session = await sessionRepository.GetByJwtIdAsync(jwtId);
                    
                    if (session == null || !session.IsValid())
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new 
                        { 
                            error = "Token has been revoked or session is invalid. Please login again." 
                        });
                        return;
                    }

                    // Update session activity
                    session.UpdateActivity();
                    await sessionRepository.UpdateAsync(session);
                    await sessionRepository.SaveChangesAsync();
                }
            }
            catch
            {
                // Invalid token format, let the authentication middleware handle it
            }
        }

        await _next(context);
    }
}

public static class TokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenValidationMiddleware>();
    }
}
