using Arkano.Transactions.Aplication.Common;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public interface IResultFactory
    {
        ResultRequest<T> Success<T>(T data, string? message = null, int? statusCode = null);

        ResultRequest<T> Created<T>(T data, string? message = null);

        ResultRequest<T> Fail<T>(IEnumerable<string> errors, int statusCode = 400, string? message = null);

        ResultRequest<T> Fail<T>(string error, int statusCode = 400, string? message = null);

        ResultRequest<Guid> TransactionCreated(Guid id, string? message = null);
    }
}