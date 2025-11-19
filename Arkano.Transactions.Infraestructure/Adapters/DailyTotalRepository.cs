using Ardalis.Specification.EntityFrameworkCore;
using Arkano.Transactions.Domain.Ports;
using Arkano.Transactions.Infraestructure.Data;
using Arkano.Transactions.Infraestructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Arkano.Transactions.Infraestructure.Adapters
{
    public class DailyTotalRepository(TransactionsDbContext dbContext) : IDailyTotalRepository
    {
        public async Task<decimal> GetDailyTotalAmountAsync(Guid accountExternalId, CancellationToken cancellationToken = default)
        {
            var spec = new DailyTransactionsTotalByAccountSpec(accountExternalId);

            var query = SpecificationEvaluator.Default.GetQuery(dbContext.Transactions.AsQueryable(), spec);

            return await query.SumAsync(transaction => transaction.Value, cancellationToken);
        }
    }
}
