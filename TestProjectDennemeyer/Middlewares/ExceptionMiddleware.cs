
using System.Net;
using System.Security.Authentication;
using System.Text.Json;


namespace TestProjectDennemeyer.Middlewares;

/// <summary>
/// This middleware is created to handle errors
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Middleware execution method that processes requests and handles exceptions.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthenticationException)
        {
            await HandleAuthenticationExceptionAsync(context);
        }
        catch (ArgumentException ex)
        {
            await HandleArgumentExceptionAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            await HandleArgumentExceptionAsync(context, ex);
        }
    }

    private static Task HandleAuthenticationExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;

        var response = new
        {
            context.Response.StatusCode,
            Message = "Authentication failed. Please provide existing user."
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static Task HandleArgumentExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

        var response = new
        {
             context.Response.StatusCode, exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}