using Ardalis.Specification;
using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Infraestructure.Specifications
{  
    public class DailyTransactionsTotalByAccountSpec : Specification<Transaction>
    {
        public DailyTransactionsTotalByAccountSpec(Guid accountExternalId)
        {
            var todayUtc = DateTime.UtcNow.Date;
            var tomorrowUtc = todayUtc.AddDays(1);

            Query.Where(transaction =>
                    transaction.SourceAccountId == accountExternalId &&
                    transaction.CreatedAt >= todayUtc &&
                    transaction.CreatedAt < tomorrowUtc)
                 .OrderBy(transaction => transaction.CreatedAt);
        }
    }
}