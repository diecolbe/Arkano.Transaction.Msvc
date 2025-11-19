using Arkano.Transactions.Aplication.Transactions.Commands;
using Arkano.Transactions.Domain.Dtos;
using Arkano.Transactions.Domain.Dtos.Events;
using Arkano.Transactions.Infraestructure.Adapters;
using MediatR;
using System.Text.Json;

namespace Arkano.Transactions.Worker.Workers
{
    public class TransactionValidatedWorker(
        IConfiguration configuration,
        ILogger<TransactionValidatedWorker> logger,
        IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase<TransactionValidatedEvent>(configuration, logger, configuration["Kafka:TransactionValidatedTopic"] ?? "transaction-validated")
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ProcessMessageAsync(TransactionValidatedEvent message, CancellationToken cancellationToken)
        {
            var validationData = JsonSerializer.Deserialize<ValidationDataDto>(message.Data.ToString()!);

            if (validationData == null)
            {
                logger.LogWarning("Error al deserializar los datos de validación desde el evento");
                return;
            }

            string status = validationData.IsValid ? "Approved" : "Rejected";

            logger.LogInformation("Procesando validación de transacción para {TransactionExternalId}: {Status} - {Reason}",
                validationData.TransactionExternalId, status, validationData.ValidationReason);

            
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();            
           
            await mediator.Send(new UpdateTransactionStatusCommand(validationData.TransactionExternalId, status), cancellationToken);
            
            logger.LogInformation("Transacción {TransactionExternalId} actualizada exitosamente a {Status}", 
                validationData.TransactionExternalId, status);
        }
    }
}
