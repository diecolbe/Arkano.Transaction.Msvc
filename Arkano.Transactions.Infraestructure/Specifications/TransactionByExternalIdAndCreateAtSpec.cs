using Ardalis.Specification;
using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Infraestructure.Specifications
{
    public class TransactionByExternalIdAndCreateAtSpec : Specification<Transaction>
    {
        public TransactionByExternalIdAndCreateAtSpec(Guid externalId, DateTime createdAt)
        {
            var date = DateOnly.FromDateTime(createdAt);
            var startUtc = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);

            Query.Where(transaction =>
                transaction.TransactionExternalId == externalId
                && transaction.CreatedAt >= startUtc
                && transaction.CreatedAt < endUtc)
                 .Take(1);
        }
    }
}
