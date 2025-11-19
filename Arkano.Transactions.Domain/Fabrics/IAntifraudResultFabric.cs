using Arkano.Transactions.Domain.Dtos;

namespace Arkano.Transactions.Domain.Fabrics
{
    public interface IAntifraudResultFabric
    {
        AntifraudResultDto Create(TransactionCreatedDto transaction);
    }
}
