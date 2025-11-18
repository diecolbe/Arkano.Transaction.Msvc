using Arkano.Transactions.Domain.Ports;
using Confluent.Kafka;
using System.Text.Json;

namespace Arkano.Transactions.Infraestructure.Adapters
{
    public class KafkaServiceBus(IProducer<Null, string> producer) : IEventBus
    {
        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = json }, cancellationToken);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}
