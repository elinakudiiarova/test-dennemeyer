
using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using System.Text.Json;


namespace TestProjectDennemeyer.Middlewares;

/// <summary>
/// This middleware is created to handle errors
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;    
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthenticationException ex)
        {
            await HandleAuthenticationExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleAuthenticationExceptionAsync(HttpContext context, AuthenticationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        
        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Authentication failed. Please provide valid credentials."
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}