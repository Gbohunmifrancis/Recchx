using System.Net;
using System.Text.Json;
using Recchx.SharedKernel.Exceptions;

namespace Recchx.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = notFoundException.Message });
                break;
            case DomainException domainException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = domainException.Message });
                break;
            case ArgumentException argumentException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = argumentException.Message });
                break;
            default:
                result = JsonSerializer.Serialize(new { error = "An internal server error occurred. Please try again later." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}

