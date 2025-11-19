using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Domain.Tests.Builders
{
    public class TransactionBuilder
    {
        private Guid _sourceAccountId = Guid.NewGuid();
        private Guid _targetAccountId = Guid.NewGuid();
        private decimal _value = 100m;

        public static TransactionBuilder Create() => new();

        public TransactionBuilder WithSourceAccount(Guid sourceAccountId)
        {
            _sourceAccountId = sourceAccountId;
            return this;
        }

        public TransactionBuilder WithTargetAccount(Guid targetAccountId)
        {
            _targetAccountId = targetAccountId;
            return this;
        }

        public TransactionBuilder WithValue(decimal value)
        {
            _value = value;
            return this;
        }

        public TransactionBuilder WithEmptySourceAccount()
        {
            _sourceAccountId = Guid.Empty;
            return this;
        }

        public TransactionBuilder WithEmptyTargetAccount()
        {
            _targetAccountId = Guid.Empty;
            return this;
        }

        public TransactionBuilder WithZeroValue()
        {
            _value = 0m;
            return this;
        }

        public TransactionBuilder WithNegativeValue(decimal negativeValue = -10m)
        {
            _value = negativeValue;
            return this;
        }

        public TransactionBuilder WithSmallPositiveValue(decimal smallValue = 0.01m)
        {
            _value = smallValue;
            return this;
        }

        public TransactionBuilder WithLargeValue(decimal largeValue = 10000m)
        {
            _value = largeValue;
            return this;
        }

        public Transaction Build()
        {
            return new Transaction(_sourceAccountId, _targetAccountId, _value);
        }

        public Transaction BuildApproved()
        {
            var transaction = Build();
            transaction.Approve();
            return transaction;
        }

        public Transaction BuildRejected()
        {
            var transaction = Build();
            transaction.Reject();
            return transaction;
        }

        public static IEnumerable<Transaction> CreateMultiple(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Create()
                    .WithSourceAccount(Guid.NewGuid())
                    .WithTargetAccount(Guid.NewGuid())
                    .WithValue(100m + i)
                    .Build();
            }
        }
    }    
}