using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Aplication.Fabrics
{
    public class TransactionFactory : ITransactionFactory
    {
        public Transaction Create(Guid sourceAccountId, Guid targetAccountId, decimal value)
        {
            Transaction transaction = new(sourceAccountId, targetAccountId, value);

            if (transaction.SourceAccountIdIsEmpty())
                throw new ArgumentException("Source account id must be provided.", nameof(sourceAccountId));

            if (transaction.TargetAccountIdIsEmpty())
                throw new ArgumentException("Target account id must be provided.", nameof(targetAccountId));

            if (transaction.ValueIsCeroOrLess())
                throw new ArgumentException("Transaction value must be greater than zero.", nameof(value));

            return transaction;
        }
    }
}