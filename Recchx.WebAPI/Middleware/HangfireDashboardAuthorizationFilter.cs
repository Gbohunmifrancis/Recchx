using Hangfire.Dashboard;

namespace Recchx.WebAPI.Middleware;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In development, allow all access
        // In production, you should implement proper authentication
        var httpContext = context.GetHttpContext();
        
        // Allow access only in development environment
        return httpContext.RequestServices
            .GetRequiredService<IWebHostEnvironment>()
            .IsDevelopment();
    }
}
