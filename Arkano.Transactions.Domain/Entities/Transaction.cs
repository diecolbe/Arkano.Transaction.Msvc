using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Domain.Entities
{
    public class Transaction(Guid sourceAccountId, Guid targetAccountId, decimal value)
    {
        private const int Zero = 0;

        public Guid TransactionExternalId { get; private set; } = Guid.NewGuid();

        public Guid SourceAccountId { get; private set; } = sourceAccountId;

        public Guid TargetAccountId { get; set; } = targetAccountId;

        public decimal Value { get; set; } = value;

        public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public bool SourceAccountIdIsEmpty() => SourceAccountId == Guid.Empty;

        public bool TargetAccountIdIsEmpty() => TargetAccountId == Guid.Empty;

        public bool ValueIsCeroOrLess() => Value <= Zero;

        public void Approve() => Status = TransactionStatus.Approved;

        public void Reject() => Status = TransactionStatus.Rejected;
    }
}
