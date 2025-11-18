using Arkano.Transactions.Domain.Dtos;
using Arkano.Transactions.Domain.Dtos.Events;
using Arkano.Transactions.Domain.Fabrics;
using Arkano.Transactions.Domain.Models;
using Arkano.Transactions.Domain.Ports;
using Arkano.Transactions.Domain.Services;
using Arkano.Transactions.Infraestructure.Adapters;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Arkano.Transactions.Antifraud.Worker.Workers
{
    public class AntifraudWorker(
        IConfiguration configuration,
        ILogger<AntifraudWorker> logger,
        AntifraudValidationService validationService,
        ITransactionValidatedDtoFabric transactionValidatedDtoFabric,
        IOptions<KafkaOptions> kafkaOptions,
        IEventBus eventBus)
        : KafkaConsumerBase<TransactionCreatedEvent>(configuration, logger, configuration["Kafka:TransactionCreatedTopic"] ?? "transaction-created")
    {
        private readonly ILogger<AntifraudWorker> _logger = logger;

        protected override async Task ProcessMessageAsync(TransactionCreatedEvent message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Procesando evento de transacción creada: {Subject}", message.Subject);

            try
            {
                if (!ValidateMessageData(message))
                {
                    return;
                }

                var transactionData = JsonSerializer.Deserialize<TransactionCreatedDto>(message.Data.ToString()!);

                if (transactionData == null)
                {
                    _logger.LogWarning("Error al deserializar los datos de la transacción desde el evento");
                    return;
                }

                _logger.LogInformation("Procesando transacción {TransactionExternalId} con valor {Value} y total diario {DailyTotal}",
                    transactionData.TransactionExternalId, transactionData.Value, transactionData.TotalValueDaily);

                var validationResult = validationService.ValidateTransaction(transactionData);
                
                var validationDto = transactionValidatedDtoFabric.Create(validationResult);
                
                await eventBus.PublishAsync(
                    kafkaOptions.Value.TransactionValidatedTopic,
                    new TransactionValidatedEvent(validationDto),
                    cancellationToken);

                _logger.LogInformation("Validación de transacción {TransactionExternalId} completada. Válida: {IsValid}, Estado: {Status}, Razón: {Reason}",
                    validationResult.TransactionExternalId,
                    validationResult.IsValid,
                    validationResult.IsValid ? "Aprobada" : "Rechazada",
                    validationResult.ValidationReason);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar los datos de la transacción del mensaje con Subject: {Subject}", message.Subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando el evento de transacción creada para Subject: {Subject}", message.Subject);
            }
        }

        private bool ValidateMessageData(TransactionCreatedEvent message)
        {
            if (message.Data is null)
            {
                _logger.LogWarning("El campo Data del mensaje es nulo");
                return false;
            }

            var dataString = message.Data.ToString();
            if (string.IsNullOrWhiteSpace(dataString))
            {
                _logger.LogWarning("El campo Data del mensaje está vacío o es solo espacios en blanco");
                return false;
            }

            return true;
        }
    }
}
