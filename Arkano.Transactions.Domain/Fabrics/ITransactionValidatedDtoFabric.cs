using Arkano.Transactions.Domain.Dtos;

namespace Arkano.Transactions.Domain.Fabrics
{
    public interface ITransactionValidatedDtoFabric
    {
        TransactionValidatedDto Create(AntifraudResultDto antifraudResult);
    }
}
