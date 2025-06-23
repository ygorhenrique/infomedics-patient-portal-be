using System.Net;

namespace InfomedicsPortal.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = exception is ArgumentException || exception is InvalidOperationException
            ? (int)HttpStatusCode.BadRequest
            : (int)HttpStatusCode.InternalServerError;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        
        string requestDetails = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
        _logger.LogError(exception, "Error processing request: {RequestDetails}", requestDetails);
        
        var errorResponse = new
        {
            error = "An error occurred",
            message = exception.Message,
            status = statusCode
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}