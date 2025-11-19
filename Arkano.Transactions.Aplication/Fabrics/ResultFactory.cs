using Arkano.Transactions.Aplication.Common;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public class ResultFactory : IResultFactory
    {
        public ResultRequest<T> Success<T>(T data, string? message = null, int? statusCode = null)
        {
            return statusCode == 201 ? 
                ResultRequest<T>.Created(data, message) : 
                ResultRequest<T>.Ok(data, message);
        }

        public ResultRequest<T> Created<T>(T data, string? message = null)
        {
            return ResultRequest<T>.Created(data, message);
        }

        public ResultRequest<T> Fail<T>(IEnumerable<string> errors, int statusCode = 400, string? message = null)
        {
            return ResultRequest<T>.Fail(errors, statusCode, message);
        }

        public ResultRequest<T> Fail<T>(string error, int statusCode = 400, string? message = null)
        {
            return ResultRequest<T>.Fail(error, statusCode, message);
        }

        public ResultRequest<Guid> TransactionCreated(Guid id, string? message = null)
        {
            return ResultRequest<Guid>.Created(id, message ?? "Transacción creada exitosamente");
        }
    }
}