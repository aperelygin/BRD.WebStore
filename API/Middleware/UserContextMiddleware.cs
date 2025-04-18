using BRD.WebStore.Catalog.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BRD.WebStore.Catalog.API.Middleware;

public class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            // Extract the user ID from the token
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                userContext.CurrentUserId = userId;
            }
        }

        await _next(context);
    }
}

public static class UserContextMiddlewareExtensions
{
    public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserContextMiddleware>();
    }
}

public class UserContext : IUserContext
{
    public int? CurrentUserId { get; set; }
}