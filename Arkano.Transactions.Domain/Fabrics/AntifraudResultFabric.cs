using Arkano.Transactions.Domain.Dtos;

namespace Arkano.Transactions.Domain.Fabrics
{
    public class AntifraudResultFabric : IAntifraudResultFabric
    {
        public AntifraudResultDto Create(TransactionCreatedDto transaction)
        {
            return new AntifraudResultDto()
            {
                TransactionExternalId = transaction.TransactionExternalId,
                TransactionValue = transaction.Value,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }
}
