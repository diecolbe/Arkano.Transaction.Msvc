using Arkano.Transactions.Domain.Tests.Builders;

namespace Arkano.Transactions.Domain.Tests.Examples
{
    public class TransactionBuilderExamples
    {
        [Fact]
        public void TransactionBuilder_BasicUsage_ShouldBeIntuitive()
        {
            // Arrange & Act
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(Guid.NewGuid())
                .WithTargetAccount(Guid.NewGuid())
                .WithValue(250.50m)
                .Build();

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(250.50m, transaction.Value);
        }

        [Fact]
        public void TransactionBuilder_ConvenienceMethods_ShouldSimplifyTesting()
        {
            // Arrange & Act
            var pendingTransaction = TransactionBuilder.Create().Build();
            var approvedTransaction = TransactionBuilder.Create().BuildApproved();
            var rejectedTransaction = TransactionBuilder.Create().BuildRejected();

            // Assert
            Assert.Equal(Arkano.Transactions.Domain.Enums.TransactionStatus.Pending, pendingTransaction.Status);
            Assert.Equal(Arkano.Transactions.Domain.Enums.TransactionStatus.Approved, approvedTransaction.Status);
            Assert.Equal(Arkano.Transactions.Domain.Enums.TransactionStatus.Rejected, rejectedTransaction.Status);
        }

        [Fact]
        public void TransactionBuilder_ValidationScenarios_ShouldBeExpressive()
        {
            // Arrange & Act
            var zeroValueTransaction = TransactionBuilder.Create()
                .WithZeroValue()
                .Build();

            var negativeTransaction = TransactionBuilder.Create()
                .WithNegativeValue(-100m)
                .Build();

            var emptySourceTransaction = TransactionBuilder.Create()
                .WithEmptySourceAccount()
                .Build();

            // Assert
            Assert.True(zeroValueTransaction.ValueIsCeroOrLess());
            Assert.True(negativeTransaction.ValueIsCeroOrLess());
            Assert.True(emptySourceTransaction.SourceAccountIdIsEmpty());
        }

        [Fact]
        public void TransactionBuilder_BatchCreation_ShouldBeEfficient()
        {
            // Arrange & Act
            var transactions = TransactionBuilder.CreateMultiple(5).ToArray();

            // Assert
            Assert.Equal(5, transactions.Length);
            Assert.All(transactions, t => Assert.NotEqual(Guid.Empty, t.TransactionExternalId));
            
            var uniqueIds = transactions.Select(t => t.TransactionExternalId).Distinct().ToArray();
            Assert.Equal(5, uniqueIds.Length);
        }

        [Fact]
        public void TransactionBuilder_EdgeCases_ShouldHandleGracefully()
        {
            // Arrange & Act
            var smallestTransaction = TransactionBuilder.Create()
                .WithSmallPositiveValue(0.001m)
                .Build();

            var largestTransaction = TransactionBuilder.Create()
                .WithLargeValue(999999.99m)
                .Build();

            // Assert
            Assert.Equal(0.001m, smallestTransaction.Value);
            Assert.Equal(999999.99m, largestTransaction.Value);
            Assert.False(smallestTransaction.ValueIsCeroOrLess());
            Assert.False(largestTransaction.ValueIsCeroOrLess());
        }

        [Fact]
        public void TransactionBuilder_ChainedOperations_ShouldMaintainFluency()
        {
            // Arrange & Act
            var complexTransaction = TransactionBuilder.Create()
                .WithSourceAccount(new Guid("12345678-1234-1234-1234-123456789012"))
                .WithTargetAccount(new Guid("87654321-4321-4321-4321-210987654321"))
                .WithValue(1337.42m)
                .Build();

            // Assert
            Assert.Equal(new Guid("12345678-1234-1234-1234-123456789012"), complexTransaction.SourceAccountId);
            Assert.Equal(new Guid("87654321-4321-4321-4321-210987654321"), complexTransaction.TargetAccountId);
            Assert.Equal(1337.42m, complexTransaction.Value);
        }

        [Fact]
        public void TransactionBuilder_ReusableBuilder_ShouldAllowModification()
        {
            // Arrange
            var baseBuilder = TransactionBuilder.Create()
                .WithSourceAccount(Guid.NewGuid())
                .WithTargetAccount(Guid.NewGuid());

            // Act
            var smallTransaction = baseBuilder.WithValue(10m).Build();
            var largeTransaction = baseBuilder.WithValue(10000m).Build();

            // Assert
            Assert.Equal(10m, smallTransaction.Value);
            Assert.Equal(10000m, largeTransaction.Value);
            Assert.NotEqual(smallTransaction.TransactionExternalId, largeTransaction.TransactionExternalId);
        }

        [Fact]
        public void TransactionBuilder_TestDataScenarios_ShouldCoverCommonCases()
        {
            // Arrange & Act
            var scenarios = new[]
            {
                TransactionBuilder.Create().WithSmallPositiveValue().Build(),
                TransactionBuilder.Create().WithValue(500m).Build(),
                TransactionBuilder.Create().WithLargeValue().Build(),
                TransactionBuilder.Create().BuildApproved(),
                TransactionBuilder.Create().BuildRejected()
            };

            // Assert
            Assert.Equal(5, scenarios.Length);
            Assert.Contains(scenarios, t => t.Status == Arkano.Transactions.Domain.Enums.TransactionStatus.Approved);
            Assert.Contains(scenarios, t => t.Status == Arkano.Transactions.Domain.Enums.TransactionStatus.Rejected);
            Assert.Contains(scenarios, t => t.Status == Arkano.Transactions.Domain.Enums.TransactionStatus.Pending);
        }
    }
}