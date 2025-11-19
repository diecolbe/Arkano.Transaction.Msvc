using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Tests.Contants;

namespace Arkano.Transactions.Domain.Tests.Helpers
{
    public static class TestDataBuilder
    {
        public static class TransactionBuilder
        {
            public static Transaction CreateValidTransaction()
            {
                return Domain.Tests.Builders.TransactionBuilder.Create()
                    .WithSourceAccount(Accounts.ValidSourceAccount)
                    .WithTargetAccount(Accounts.ValidTargetAccount)
                    .WithValue(100m)
                    .Build();
            }

            public static Transaction CreateTransactionWithAmount(decimal amount)
            {
                return Domain.Tests.Builders.TransactionBuilder.Create()
                    .WithSourceAccount(Accounts.ValidSourceAccount)
                    .WithTargetAccount(Accounts.ValidTargetAccount)
                    .WithValue(amount)
                    .Build();
            }

            public static Transaction CreateTransactionWithAccounts(Guid sourceAccountId, Guid targetAccountId)
            {
                return Domain.Tests.Builders.TransactionBuilder.Create()
                    .WithSourceAccount(sourceAccountId)
                    .WithTargetAccount(targetAccountId)
                    .WithValue(100m)
                    .Build();
            }

            public static Transaction CreateTransactionWithStatus(TransactionStatus status)
            {
                return status switch
                {
                    TransactionStatus.Approved => Domain.Tests.Builders.TransactionBuilder.Create()
                        .WithSourceAccount(Accounts.ValidSourceAccount)
                        .WithTargetAccount(Accounts.ValidTargetAccount)
                        .BuildApproved(),
                    TransactionStatus.Rejected => Domain.Tests.Builders.TransactionBuilder.Create()
                        .WithSourceAccount(Accounts.ValidSourceAccount)
                        .WithTargetAccount(Accounts.ValidTargetAccount)
                        .BuildRejected(),
                    TransactionStatus.Pending => Domain.Tests.Builders.TransactionBuilder.Create()
                        .WithSourceAccount(Accounts.ValidSourceAccount)
                        .WithTargetAccount(Accounts.ValidTargetAccount)
                        .Build(),
                    _ => throw new ArgumentException($"Estado no soportado: {status}")
                };
            }

            public static IEnumerable<Transaction> CreateMultipleTransactions(int count)
            {
                return Domain.Tests.Builders.TransactionBuilder.CreateMultiple(count);
            }

            public static Transaction CreateTransactionWithEmptySourceAccount()
            {
                return Domain.Tests.Builders.TransactionBuilder.Create()
                    .WithEmptySourceAccount()
                    .WithTargetAccount(Accounts.ValidTargetAccount)
                    .WithValue(100m)
                    .Build();
            }

            public static Transaction CreateTransactionWithEmptyTargetAccount()
            {
                return Domain.Tests.Builders.TransactionBuilder.Create()
                    .WithSourceAccount(Accounts.ValidSourceAccount)
                    .WithEmptyTargetAccount()
                    .WithValue(100m)
                    .Build();
            }
        }
    }
}