using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Tests.Builders;

namespace Arkano.Transactions.Domain.Tests.BusinessRules
{
    public class BusinessRulesTests
    {
        [Fact]
        public void TransactionBusinessRules_ShouldEnforceValidAccounts()
        {
            // Arrange
            var factory = new TransactionFactory();
            var validSourceAccount = Guid.NewGuid();
            var validTargetAccount = Guid.NewGuid();

            // Act & Assert
            var validTransaction = TransactionBuilder.Create()
                .WithSourceAccount(validSourceAccount)
                .WithTargetAccount(validTargetAccount)
                .WithValue(100m)
                .Build();

            Assert.NotNull(validTransaction);
            Assert.Equal(validSourceAccount, validTransaction.SourceAccountId);
            Assert.Equal(validTargetAccount, validTransaction.TargetAccountId);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                factory.Create(Guid.Empty, validTargetAccount, 100m));

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                factory.Create(validSourceAccount, Guid.Empty, 100m));
        }

        [Fact]
        public void TransactionBusinessRules_ShouldEnforcePositiveAmounts()
        {
            // Arrange & Act & Assert
            var validTransaction = TransactionBuilder.Create()
                .WithSmallPositiveValue(0.01m)
                .Build();

            Assert.NotNull(validTransaction);
            Assert.Equal(0.01m, validTransaction.Value);

            // Act & Assert
            var factory = new TransactionFactory();
            Assert.Throws<ArgumentException>(() =>
                factory.Create(Guid.NewGuid(), Guid.NewGuid(), 0m));

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                factory.Create(Guid.NewGuid(), Guid.NewGuid(), -10m));
        }

        [Fact]
        public void TransactionBusinessRules_ShouldInitializeWithPendingStatus()
        {
            // Arrange & Act
            var transaction = TransactionBuilder.Create()
                .WithValue(100m)
                .Build();

            // Assert
            Assert.Equal(TransactionStatus.Pending, transaction.Status);
            Assert.Equal(DateTimeKind.Utc, transaction.CreatedAt.Kind);
            Assert.True(transaction.CreatedAt <= DateTime.UtcNow);
            Assert.True(transaction.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void TransactionBusinessRules_ShouldAllowStatusTransitions()
        {
            // Arrange & Act & Assert
            var approvedTransaction = TransactionBuilder.Create()
                .WithValue(100m)
                .BuildApproved();

            Assert.Equal(TransactionStatus.Approved, approvedTransaction.Status);

            // Arrange & Act & Assert
            var rejectedTransaction = TransactionBuilder.Create()
                .WithValue(100m)
                .BuildRejected();

            Assert.Equal(TransactionStatus.Rejected, rejectedTransaction.Status);
        }

        [Fact]
        public void TransactionBusinessRules_ShouldGenerateUniqueExternalIds()
        {
            // Arrange & Act
            var transactions = TransactionBuilder.CreateMultiple(3).ToArray();

            // Assert
            Assert.NotEqual(transactions[0].TransactionExternalId, transactions[1].TransactionExternalId);
            Assert.NotEqual(transactions[0].TransactionExternalId, transactions[2].TransactionExternalId);
            Assert.NotEqual(transactions[1].TransactionExternalId, transactions[2].TransactionExternalId);

            Assert.All(transactions, t => Assert.NotEqual(Guid.Empty, t.TransactionExternalId));
        }

        [Fact]
        public void TransactionBusinessRules_ShouldValidateAccountIdsCorrectly()
        {
            // Arrange & Act
            var transactionEmptySource = TransactionBuilder.Create()
                .WithEmptySourceAccount()
                .Build();

            var transactionEmptyTarget = TransactionBuilder.Create()
                .WithEmptyTargetAccount()
                .Build();

            var transactionValid = TransactionBuilder.Create()
                .Build();

            // Assert
            Assert.True(transactionEmptySource.SourceAccountIdIsEmpty());
            Assert.False(transactionEmptySource.TargetAccountIdIsEmpty());

            Assert.False(transactionEmptyTarget.SourceAccountIdIsEmpty());
            Assert.True(transactionEmptyTarget.TargetAccountIdIsEmpty());

            Assert.False(transactionValid.SourceAccountIdIsEmpty());
            Assert.False(transactionValid.TargetAccountIdIsEmpty());
        }

        [Fact]
        public void TransactionBusinessRules_ShouldValidateAmountsCorrectly()
        {
            // Arrange & Acc
            var transactionPositive = TransactionBuilder.Create()
                .WithValue(100m)
                .Build();

            var transactionZero = TransactionBuilder.Create()
                .WithZeroValue()
                .Build();

            var transactionNegative = TransactionBuilder.Create()
                .WithNegativeValue(-50m)
                .Build();

            var transactionSmall = TransactionBuilder.Create()
                .WithSmallPositiveValue(0.001m)
                .Build();

            // Assert
            Assert.False(transactionPositive.ValueIsCeroOrLess());
            Assert.True(transactionZero.ValueIsCeroOrLess());
            Assert.True(transactionNegative.ValueIsCeroOrLess());
            Assert.False(transactionSmall.ValueIsCeroOrLess());
        }

        [Fact]
        public void TransactionBusinessRules_ShouldSupportFluentSyntax()
        {
            // Arrange & Ac
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(Guid.NewGuid())
                .WithTargetAccount(Guid.NewGuid())
                .WithLargeValue(5000m)
                .Build();

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(5000m, transaction.Value);
            Assert.Equal(TransactionStatus.Pending, transaction.Status);
        }

        [Fact]
        public void TransactionBusinessRules_ShouldCreateTransactionsWithDifferentStates()
        {
            // Arrange & Act
            var pendingTransaction = TransactionBuilder.Create().Build();
            var approvedTransaction = TransactionBuilder.Create().BuildApproved();
            var rejectedTransaction = TransactionBuilder.Create().BuildRejected();

            // Assert
            Assert.Equal(TransactionStatus.Pending, pendingTransaction.Status);
            Assert.Equal(TransactionStatus.Approved, approvedTransaction.Status);
            Assert.Equal(TransactionStatus.Rejected, rejectedTransaction.Status);
        }
    }
}