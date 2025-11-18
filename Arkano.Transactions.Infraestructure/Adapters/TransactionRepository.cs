using Ardalis.Specification.EntityFrameworkCore;
using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Domain.Ports;
using Arkano.Transactions.Infraestructure.Data;
using Arkano.Transactions.Infraestructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Arkano.Transactions.Infraestructure.Adapters
{
    public class TransactionRepository(TransactionsDbContext dbContext) : ITransactionRepository
    {

        public async Task<Guid> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            dbContext.Transactions.Add(transaction);
            await dbContext.SaveChangesAsync(cancellationToken);
            return transaction.TransactionExternalId;
        }

        public async Task<Transaction?> GetByExternalIdAndCreateAtAsync(Guid externalId, DateTime createdAt, CancellationToken cancellationToken = default)
        {
            var spec = new TransactionByExternalIdAndCreateAtSpec(externalId, createdAt);

            var query = SpecificationEvaluator.Default.GetQuery(dbContext.Transactions.AsQueryable(), spec);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Transaction?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        {
            var spec = new TransactionByExternalIdSpec(externalId);

            var query = SpecificationEvaluator.Default.GetQuery(dbContext.Transactions.AsQueryable(), spec);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            dbContext.Transactions.Update(transaction);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
