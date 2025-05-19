using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Exceptions
{
    public class NotFoundException(string message = "Not found exception", int statusCode = StatusCodes.Status400BadRequest) : Exception(message)
    {
        public int StatusCode { get; set; } = statusCode;
    }
}
