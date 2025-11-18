using Arkano.Transactions.Domain.Dtos;

namespace Arkano.Transactions.Domain.Fabrics
{
    public class TransactionValidatedDtoFabric : ITransactionValidatedDtoFabric
    {
        public TransactionValidatedDto Create(AntifraudResultDto antifraudResult)
        {
            return new TransactionValidatedDto(
                TransactionExternalId: antifraudResult.TransactionExternalId,
                IsValid: antifraudResult.IsValid,
                ValidationReason: antifraudResult.ValidationReason,
                ProcessedAt: antifraudResult.ProcessedAt,
                Status: antifraudResult.IsValid ? "Approved" : "Rejected"
            );
        }
    }
}