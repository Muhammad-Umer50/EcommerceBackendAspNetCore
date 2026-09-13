using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Errors
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "An unexpected error occurred.",
                Detail = exception.Message,
            };

            // 4. Set HTTP response configurations
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/problem+json";

            // 5. Stream the JSON payload back to the client
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // Return true to signal that this exception has been completely handled
            return true;
        }
    }
}
