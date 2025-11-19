using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Domain.Tests.Fabrics
{
    public class TransactionFactoryTests
    {
        private readonly TransactionFactory _transactionFactory;
        private readonly Guid _validSourceAccountId = Guid.NewGuid();
        private readonly Guid _validTargetAccountId = Guid.NewGuid();
        private const decimal ValidValue = 250.75m;

        public TransactionFactoryTests()
        {
            _transactionFactory = new TransactionFactory();
        }

        [Fact]
        public void Create_ShouldReturnValidTransaction_WhenAllParametersAreValid()
        {
            // Act
            var result = _transactionFactory.Create(_validSourceAccountId, _validTargetAccountId, ValidValue);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_validSourceAccountId, result.SourceAccountId);
            Assert.Equal(_validTargetAccountId, result.TargetAccountId);
            Assert.Equal(ValidValue, result.Value);
            Assert.NotEqual(Guid.Empty, result.TransactionExternalId);
            Assert.Equal(TransactionStatus.Pending, result.Status);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenSourceAccountIdIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _transactionFactory.Create(Guid.Empty, _validTargetAccountId, ValidValue));
            
            Assert.Equal("sourceAccountId", exception.ParamName);
            Assert.Contains("Source account id must be provided", exception.Message);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenTargetAccountIdIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _transactionFactory.Create(_validSourceAccountId, Guid.Empty, ValidValue));
            
            Assert.Equal("targetAccountId", exception.ParamName);
            Assert.Contains("Target account id must be provided", exception.Message);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenValueIsZero()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _transactionFactory.Create(_validSourceAccountId, _validTargetAccountId, 0));
            
            Assert.Equal("value", exception.ParamName);
            Assert.Contains("Transaction value must be greater than zero", exception.Message);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenValueIsNegative()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _transactionFactory.Create(_validSourceAccountId, _validTargetAccountId, -10.50m));
            
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_ShouldCreateUniqueTransactions_WhenCalledMultipleTimes()
        {
            // Act
            var transaction1 = _transactionFactory.Create(_validSourceAccountId, _validTargetAccountId, ValidValue);
            var transaction2 = _transactionFactory.Create(_validSourceAccountId, _validTargetAccountId, ValidValue);

            // Assert
            Assert.NotEqual(transaction1.TransactionExternalId, transaction2.TransactionExternalId);
        }
    }
}