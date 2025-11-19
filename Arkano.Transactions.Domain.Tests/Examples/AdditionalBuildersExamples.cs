using Arkano.Transactions.Domain.Tests.Builders;

namespace Arkano.Transactions.Domain.Tests.Examples
{
    public class AdditionalBuildersExamples
    {
        [Fact]
        public void AccountBuilder_ShouldCreateValidAccounts()
        {
            // Arrange & Act
            var account = AccountBuilder.Create()
                .WithOwnerName("John Doe")
                .Build();

            // Assert
            Assert.NotNull(account);
            Assert.Equal("John Doe", account.OwnerName);
            Assert.NotEqual(Guid.Empty, account.AccountId);
        }

        [Fact]
        public void AccountBuilder_ValidationScenarios_ShouldHandleEdgeCases()
        {
            // Arrange & Act
            var emptyNameAccount = AccountBuilder.Create()
                .WithEmptyOwnerName()
                .Build();

            var longNameAccount = AccountBuilder.Create()
                .WithLongOwnerName()
                .Build();

            var specialCharsAccount = AccountBuilder.Create()
                .WithSpecialCharactersInName()
                .Build();

            // Assert
            Assert.Equal(string.Empty, emptyNameAccount.OwnerName);
            Assert.Equal(300, longNameAccount.OwnerName.Length);
            Assert.Contains("@#$%^&*()", specialCharsAccount.OwnerName);
        }

        [Fact]
        public void CommandBuilders_ShouldCreateValidCommands()
        {
            // Arrange & Act
            var createCommand = CreateTransactionCommandBuilder.Create()
                .WithLargeAmount()
                .Build();

            var updateCommand = UpdateTransactionStatusCommandBuilder.Create()
                .WithRejectedStatus()
                .Build();

            // Assert
            Assert.Equal(50000m, createCommand.Value);
            Assert.Equal("Rejected", updateCommand.Status);
        }

        [Fact]
        public void ConfigurationBuilders_ShouldCreateValidKafkaOptions()
        {
            // Arrange & Act
            var kafkaOptions = KafkaOptionsBuilder.Create()
                .WithTestEnvironmentDefaults()
                .WithGroupId("test-group-123")
                .Build();

            // Assert
            Assert.Equal("test-group-123", kafkaOptions.GroupId);
            Assert.Equal(1000, kafkaOptions.MessageTimeoutMs);
            Assert.Equal("localhost:9092", kafkaOptions.BootstrapServers);
        }

        [Fact]
        public void MultipleBuilders_ShouldWorkTogether()
        {
            // Arrange
            var sourceAccount = AccountBuilder.Create()
                .WithOwnerName("Source Owner")
                .Build();

            var targetAccount = AccountBuilder.Create()
                .WithOwnerName("Target Owner")
                .Build();

            // Act
            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(sourceAccount.AccountId)
                .WithTargetAccount(targetAccount.AccountId)
                .WithValue(500m)
                .Build();

            var command = CreateTransactionCommandBuilder.Create()
                .WithSourceAccountId(sourceAccount.AccountId)
                .WithTargetAccountId(targetAccount.AccountId)
                .WithAmount(500m)
                .Build();

            // Assert
            Assert.Equal(sourceAccount.AccountId, transaction.SourceAccountId);
            Assert.Equal(targetAccount.AccountId, transaction.TargetAccountId);
            Assert.Equal(sourceAccount.AccountId, command.SourceAccountId);
            Assert.Equal(targetAccount.AccountId, command.TargetAccountId);
            Assert.Equal(500m, command.Value);
        }

        [Fact]
        public void BatchCreation_ShouldWorkForAllBuilders()
        {
            // Arrange & Act
            var accounts = AccountBuilder.CreateMultiple(3).ToArray();
            var transactions = TransactionBuilder.CreateMultiple(3).ToArray();

            // Assert
            Assert.Equal(3, accounts.Length);
            Assert.Equal(3, transactions.Length);
            Assert.All(accounts, a => Assert.NotEqual(Guid.Empty, a.AccountId));
            Assert.All(transactions, t => Assert.NotEqual(Guid.Empty, t.TransactionExternalId));
        }

        [Fact]
        public void BuilderPattern_DemonstratesPowerOfFluency()
        {
            // Arrange & Act - Complex scenario with multiple builders
            var sourceAccount = AccountBuilder.Create()
                .WithOwnerName("Premium Customer")
                .Build();

            var transaction = TransactionBuilder.Create()
                .WithSourceAccount(sourceAccount.AccountId)
                .WithLargeValue(5000m)
                .BuildApproved();

            var command = CreateTransactionCommandBuilder.Create()
                .WithSourceAccountId(sourceAccount.AccountId)
                .WithLargeAmount()
                .Build();

            // Assert
            Assert.Equal("Premium Customer", sourceAccount.OwnerName);
            Assert.Equal(Arkano.Transactions.Domain.Enums.TransactionStatus.Approved, transaction.Status);
            Assert.Equal(5000m, transaction.Value);
            Assert.Equal(50000m, command.Value);
        }
    }
}