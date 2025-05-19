using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Exceptions
{
    public class BadRequestException(string message = "Bad request exception", int statusCode = StatusCodes.Status400BadRequest) : Exception(message)
    {
        public int StatusCode { get; set; } = statusCode;
    }
}
