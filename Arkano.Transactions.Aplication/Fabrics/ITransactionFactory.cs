using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public interface ITransactionFactory
    {
        Transaction Create(Guid sourceAccountId, Guid targetAccountId, decimal value);
    }
}