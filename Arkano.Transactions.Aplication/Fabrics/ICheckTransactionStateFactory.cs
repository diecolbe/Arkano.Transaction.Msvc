using Arkano.Transactions.Aplication.Dtos;
using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public interface ICheckTransactionStateFactory
    {
        CheckTransactionStateDto FromStatus(TransactionStatus status);
    }
}