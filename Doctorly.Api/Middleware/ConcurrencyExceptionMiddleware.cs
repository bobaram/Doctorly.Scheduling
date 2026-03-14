using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Doctorly.Api.Middleware;

public class ConcurrencyExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ConcurrencyExceptionMiddleware> _logger;

    public ConcurrencyExceptionMiddleware(RequestDelegate next, ILogger<ConcurrencyExceptionMiddleware> logger)
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
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict detected");
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await context.Response.WriteAsJsonAsync(new { Error = "The data has been modified by another user. Please reload and try again." });
        }
    }
}
