using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Tests.Builders;

namespace Arkano.Transactions.Domain.Tests.Entities
{
    public class TransactionTests
    {
        private readonly Guid _validSourceAccountId = Guid.NewGuid();
        private readonly Guid _validTargetAccountId = Guid.NewGuid();
        private const decimal ValidValue = 100.50m;

        [Fact]
        public void Transaction_Constructor_ShouldCreateValidTransaction()
        {
            // Act
            var transaction = new Transaction(_validSourceAccountId, _validTargetAccountId, ValidValue);

            // Assert
            Assert.Equal(_validSourceAccountId, transaction.SourceAccountId);
            Assert.Equal(_validTargetAccountId, transaction.TargetAccountId);
            Assert.Equal(ValidValue, transaction.Value);
            Assert.Equal(TransactionStatus.Pending, transaction.Status);
            Assert.NotEqual(Guid.Empty, transaction.TransactionExternalId);
            Assert.Equal(DateTimeKind.Utc, transaction.CreatedAt.Kind);
        }

        [Fact]
        public void Transaction_UsingBuilder_ShouldCreateValidTransaction()
        {
            // Act
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(_validSourceAccountId)
                .WithTargetAccount(_validTargetAccountId)
                .WithValue(ValidValue)
                .Build();

            // Assert
            Assert.Equal(_validSourceAccountId, transaction.SourceAccountId);
            Assert.Equal(_validTargetAccountId, transaction.TargetAccountId);
            Assert.Equal(ValidValue, transaction.Value);
            Assert.Equal(TransactionStatus.Pending, transaction.Status);
            Assert.NotEqual(Guid.Empty, transaction.TransactionExternalId);
            Assert.Equal(DateTimeKind.Utc, transaction.CreatedAt.Kind);
        }

        [Fact]
        public void Transaction_SourceAccountIdIsEmpty_ShouldReturnTrue_WhenSourceAccountIdIsEmpty()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithEmptySourceAccount()
                .WithTargetAccount(_validTargetAccountId)
                .WithValue(ValidValue)
                .Build();

            // Act & Assert
            Assert.True(transaction.SourceAccountIdIsEmpty());
        }

        [Fact]
        public void Transaction_SourceAccountIdIsEmpty_ShouldReturnFalse_WhenSourceAccountIdIsValid()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(_validSourceAccountId)
                .WithTargetAccount(_validTargetAccountId)
                .WithValue(ValidValue)
                .Build();

            // Act & Assert
            Assert.False(transaction.SourceAccountIdIsEmpty());
        }

        [Fact]
        public void Transaction_TargetAccountIdIsEmpty_ShouldReturnTrue_WhenTargetAccountIdIsEmpty()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(_validSourceAccountId)
                .WithEmptyTargetAccount()
                .WithValue(ValidValue)
                .Build();

            // Act & Assert
            Assert.True(transaction.TargetAccountIdIsEmpty());
        }

        [Fact]
        public void Transaction_ValueIsCeroOrLess_ShouldReturnTrue_WhenValueIsZero()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithZeroValue()
                .Build();

            // Act & Assert
            Assert.True(transaction.ValueIsCeroOrLess());
        }

        [Fact]
        public void Transaction_ValueIsCeroOrLess_ShouldReturnTrue_WhenValueIsNegative()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithNegativeValue(-10m)
                .Build();

            // Act & Assert
            Assert.True(transaction.ValueIsCeroOrLess());
        }

        [Fact]
        public void Transaction_ValueIsCeroOrLess_ShouldReturnFalse_WhenValueIsPositive()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(ValidValue)
                .Build();

            // Act & Assert
            Assert.False(transaction.ValueIsCeroOrLess());
        }

        [Fact]
        public void Transaction_Approve_ShouldChangeStatusToApproved()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(ValidValue)
                .Build();

            // Act
            transaction.Approve();

            // Assert
            Assert.Equal(TransactionStatus.Approved, transaction.Status);
        }

        [Fact]
        public void Transaction_Reject_ShouldChangeStatusToRejected()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(ValidValue)
                .Build();

            // Act
            transaction.Reject();

            // Assert
            Assert.Equal(TransactionStatus.Rejected, transaction.Status);
        }

        [Fact]
        public void Transaction_Builder_ShouldSupportMethodChaining()
        {
            // Act
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(Guid.NewGuid())
                .WithTargetAccount(Guid.NewGuid())
                .WithLargeValue(10000m)
                .Build();

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(10000m, transaction.Value);
            Assert.NotEqual(Guid.Empty, transaction.SourceAccountId);
            Assert.NotEqual(Guid.Empty, transaction.TargetAccountId);
        }

        [Fact]
        public void Transaction_Builder_ShouldCreatePreApprovedTransaction()
        {
            // Act
            var approvedTransaction = TransactionBuilder.Create()
                .WithValue(ValidValue)
                .BuildApproved();

            // Assert
            Assert.Equal(TransactionStatus.Approved, approvedTransaction.Status);
            Assert.Equal(ValidValue, approvedTransaction.Value);
        }
    }
}