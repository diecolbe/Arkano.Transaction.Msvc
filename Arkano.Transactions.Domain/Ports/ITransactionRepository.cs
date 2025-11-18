using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Domain.Ports
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByExternalIdAndCreateAtAsync(Guid externalId, DateTime createdAt, CancellationToken cancellationToken = default);

        Task<Transaction?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);

        Task<Guid> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);

        Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
