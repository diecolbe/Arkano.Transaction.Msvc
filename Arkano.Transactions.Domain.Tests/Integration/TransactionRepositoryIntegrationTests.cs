using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Tests.Builders;
using Arkano.Transactions.Infraestructure.Adapters;
using Arkano.Transactions.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Arkano.Transactions.Domain.Tests.Integration
{
    public sealed class TransactionRepositoryIntegrationTests : IDisposable
    {
        private readonly TransactionsDbContext _context;
        private readonly TransactionRepository _repository;

        public TransactionRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<TransactionsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TransactionsDbContext(options);
            _repository = new TransactionRepository(_context);
        }


        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnTransactionId()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(100m)
                .Build();

            // Act
            var result = await _repository.CreateAsync(transaction);

            // Assert
            Assert.Equal(transaction.TransactionExternalId, result);

            var savedTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionExternalId == result);

            Assert.NotNull(savedTransaction);
            Assert.Equal(transaction.Value, savedTransaction.Value);
        }

        [Fact]
        public async Task GetByExternalIdAsync_ShouldReturnTransaction_WhenExists()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(150m)
                .Build();

            await _repository.CreateAsync(transaction);

            // Act
            var result = await _repository.GetByExternalIdAsync(transaction.TransactionExternalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transaction.TransactionExternalId, result.TransactionExternalId);
            Assert.Equal(transaction.Value, result.Value);
        }

        [Fact]
        public async Task GetByExternalIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByExternalIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTransactionStatus()
        {
            // Arrange
            var transaction = TransactionBuilder.Create()
                .WithValue(200m)
                .Build();

            await _repository.CreateAsync(transaction);

            // Act
            transaction.Approve();
            await _repository.UpdateAsync(transaction);

            // Assert
            var updatedTransaction = await _repository.GetByExternalIdAsync(transaction.TransactionExternalId);
            Assert.NotNull(updatedTransaction);
            Assert.Equal(Arkano.Transactions.Domain.Enums.TransactionStatus.Approved, updatedTransaction.Status);
        }

        [Fact]
        public async Task Repository_ShouldHandleMultipleTransactions()
        {
            // Arrange
            var transactions = TransactionBuilder.CreateMultiple(3).ToArray();

            // Act
            foreach (var transaction in transactions)
            {
                await _repository.CreateAsync(transaction);
            }

            // Assert
            foreach (var transaction in transactions)
            {
                var savedTransaction = await _repository.GetByExternalIdAsync(transaction.TransactionExternalId);
                Assert.NotNull(savedTransaction);
                Assert.Equal(transaction.Value, savedTransaction.Value);
            }
        }

        [Fact]
        public async Task Repository_ShouldWorkWithPreBuiltTransactionStates()
        {
            // Arrange
            var pendingTransaction = TransactionBuilder.Create().Build();
            var approvedTransaction = TransactionBuilder.Create().BuildApproved();
            var rejectedTransaction = TransactionBuilder.Create().BuildRejected();

            // Act
            await _repository.CreateAsync(pendingTransaction);
            await _repository.CreateAsync(approvedTransaction);
            await _repository.CreateAsync(rejectedTransaction);

            // Assert
            var savedPending = await _repository.GetByExternalIdAsync(pendingTransaction.TransactionExternalId);
            var savedApproved = await _repository.GetByExternalIdAsync(approvedTransaction.TransactionExternalId);
            var savedRejected = await _repository.GetByExternalIdAsync(rejectedTransaction.TransactionExternalId);

            Assert.Equal(TransactionStatus.Pending, savedPending!.Status);
            Assert.Equal(TransactionStatus.Approved, savedApproved!.Status);
            Assert.Equal(TransactionStatus.Rejected, savedRejected!.Status);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}