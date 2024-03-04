// Custom middleware to check authentication
public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the user is authenticated
        if (context.Session.GetInt32("userId") == null)
        {
            // Redirect to the login page or perform other actions
            context.Response.Redirect("/Home/Index");
            return;
        }

        // Continue processing the request
        await _next(context);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }
}
