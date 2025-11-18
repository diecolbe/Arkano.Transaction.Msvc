using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Arkano.Transactions.Infraestructure.Adapters
{
    public abstract class KafkaConsumerBase<TEvent> : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string _topic;
        private readonly ILogger _logger;

        protected KafkaConsumerBase(IConfiguration configuration, ILogger logger, string topic)
        {
            _topic = topic;
            _logger = logger;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"] ?? "default-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);
            _logger.LogInformation("Suscrito al tópico de Kafka: {Topic}", _topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    _logger.LogDebug("Mensaje recibido del tópico {Topic}: {Message}", _topic, result.Message.Value);

                    try
                    {
                        var message = JsonSerializer.Deserialize<TEvent>(result.Message.Value);

                        if (message is not null)
                        {
                            await ProcessMessageAsync(message, stoppingToken);

                            _consumer.StoreOffset(result);
                            _consumer.Commit(result);

                            _logger.LogDebug("Mensaje procesado exitosamente y confirmado");
                        }
                        else
                        {
                            _logger.LogWarning("El mensaje deserializado es nulo - descartando mensaje");
                            _consumer.StoreOffset(result);
                            _consumer.Commit(result);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Error de deserialización - descartando mensaje mal formado del tópico {Topic}", _topic);
                        _consumer.StoreOffset(result);
                        _consumer.Commit(result);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando mensaje del tópico {Topic} - el mensaje será reintentado", _topic);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consumiendo mensajes del tópico {Topic}", _topic);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex, "Consumo cancelado para el tópico {Topic}", _topic);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inesperado en el consumidor del tópico {Topic}", _topic);
                }
            }
        }

        protected abstract Task ProcessMessageAsync(TEvent message, CancellationToken cancellationToken);

        public override void Dispose()
        {
            try
            {
                _consumer?.Close();
                _consumer?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al liberar recursos del consumidor de Kafka");
            }
            finally
            {
                base.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
