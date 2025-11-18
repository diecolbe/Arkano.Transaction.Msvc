using Arkano.Transactions.Domain.Attributes;
using Arkano.Transactions.Domain.Dtos;
using Arkano.Transactions.Domain.Dtos.Events;
using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Models;
using Arkano.Transactions.Domain.Ports;
using Microsoft.Extensions.Options;

namespace Arkano.Transactions.Domain.Services
{
    [DomainService]
    public class TransactionService(ITransactionRepository transactionRepository, IEventBus eventBus, IOptions<KafkaOptions> kafkaOptions)
    {
        private readonly KafkaOptions _kafkaOptions = kafkaOptions.Value;

        public async Task<Guid> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            var transactionId = await transactionRepository.CreateAsync(transaction, cancellationToken);

            await eventBus.PublishAsync(_kafkaOptions.TransactionCreatedTopic,
                new TransactionCreatedEvent(
                    new TransactionCreatedDto(
                        transaction.TransactionExternalId,
                        transaction.Value,
                        nameof(transaction.Status),
                        transaction.Value)),
                cancellationToken);

            return transactionId;
        }

        public async Task<Transaction?> GetByExternalIdAndCreateAtAsync(Guid externalId, DateTime createAt, CancellationToken cancellationToken = default)
        {
            return await transactionRepository.GetByExternalIdAndCreateAtAsync(externalId, createAt, cancellationToken);
        }

        public async Task<TransactionStatus> GetTransactionStatusByIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        {
            var transaction = await transactionRepository.GetByExternalIdAsync(externalId, cancellationToken);
            
            if (transaction == null)
            {
                throw new ArgumentException($"Transaction with external ID {externalId} not found.");
            }

            return transaction.Status;
        }
    }
}