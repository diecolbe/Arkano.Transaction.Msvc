using Arkano.Transactions.Domain.Attributes;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Exceptions;
using Arkano.Transactions.Domain.Ports;

namespace Arkano.Transactions.Domain.Services
{
    [DomainService]
    public class TransactionStatusService(ITransactionRepository transactionRepository)
    {
        public async Task Update(Guid transactionExternalId, string status, CancellationToken cancelationToken)
        {
            var transaction = await transactionRepository.GetByExternalIdAsync(transactionExternalId, cancelationToken)
                ?? throw new TransactionNotFoundException("Transaction not found");

            switch (status)
            {
                case nameof(TransactionStatus.Approved):
                    transaction.Approve();
                    break;
                case nameof(TransactionStatus.Rejected):
                    transaction.Reject();
                    break;
                default:
                    throw new ArgumentException("Invalid status");
            }

            await transactionRepository.UpdateAsync(transaction, cancelationToken);
        }
    }
}
