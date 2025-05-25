using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            //2nd way to use exception handling with IExceptionHandler

            //var (detail, title, statusCode)= exception switch
            //{
            //    NotFoundException e => (e.Message, exception.GetType().Name, e.StatusCode),
            //    BadRequestException e => (e.Message, exception.GetType().Name, e.StatusCode),
            //    InternalServerException e => (e.Message, exception.GetType().Name, e.StatusCode),
            //    _ => (exception.Message, exception.GetType().Name, (int)HttpStatusCode.InternalServerError),
            //};

            //logger.LogError("Problem occured");
            //var problemDetails = new ProblemDetails
            //{
            //    Type =title,
            //    Title = $"Exception: {title}",
            //    Status = statusCode,
            //    Detail = detail,
            //    Instance = httpContext.Request.Path,
            //};
            //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
