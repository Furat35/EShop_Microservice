using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Exceptions
{
    public class InternalServerException(string message = "Internal server exception", int statusCode = StatusCodes.Status500InternalServerError) : Exception(message)
    {
        public int StatusCode { get; set; } = statusCode;
    }
}
