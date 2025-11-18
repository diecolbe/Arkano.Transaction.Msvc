using System.Net;

namespace Arkano.Transactions.Aplication.Common
{
    public record ResultRequest<T>(
        bool Success,
        T? Data,
        IEnumerable<string>? Errors,
        string? Message,
        int StatusCode,
        DateTime Timestamp)
    {
        public static ResultRequest<T> Ok(T data, string? message = null) =>
            new(true, data, null, message, (int)HttpStatusCode.OK, DateTime.UtcNow);

        public static ResultRequest<T> Created(T data, string? message = null) =>
            new(true, data, null, message, (int)HttpStatusCode.Created, DateTime.UtcNow);

        public static ResultRequest<T> Fail(IEnumerable<string> errors, int statusCode = (int)HttpStatusCode.BadRequest, string? message = null) =>
            new(false, default, errors, message, statusCode, DateTime.UtcNow);

        public static ResultRequest<T> Fail(string error, int statusCode = (int)HttpStatusCode.BadRequest, string? message = null) =>
            Fail([error], statusCode, message);
    }
}
