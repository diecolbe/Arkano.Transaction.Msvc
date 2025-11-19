using Arkano.Transactions.Aplication.Dtos;
using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public class CheckTransactionStateFactory : ICheckTransactionStateFactory
    {
        public CheckTransactionStateDto FromStatus(TransactionStatus status)
        {
            return status switch
            {
                TransactionStatus.Pending => new CheckTransactionStateDto(TransactionStatus.Pending),
                TransactionStatus.Approved => new CheckTransactionStateDto(TransactionStatus.Approved),
                TransactionStatus.Rejected => new CheckTransactionStateDto(TransactionStatus.Rejected),
                _ => throw new ArgumentOutOfRangeException(nameof(status), "Invalid transaction status.")
            };
        }
    }
}