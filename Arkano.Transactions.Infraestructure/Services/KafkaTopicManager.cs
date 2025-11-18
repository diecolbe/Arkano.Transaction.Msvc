using Arkano.Transactions.Domain.Models;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Arkano.Transactions.Infraestructure.Services
{
    public class KafkaTopicManager(IOptions<KafkaOptions> kafkaOptions, ILogger<KafkaTopicManager> logger)
    {

        public async Task CreateTopicsAsync()
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = kafkaOptions.Value.BootstrapServers
            };

            using var adminClient = new AdminClientBuilder(config).Build();

            var topics = new List<TopicSpecification>
            {
                new() {
                    Name = kafkaOptions.Value.TransactionCreatedTopic,
                    NumPartitions = 3,
                    ReplicationFactor = 1
                },
                new TopicSpecification
                {
                    Name = kafkaOptions.Value.TransactionValidatedTopic,
                    NumPartitions = 3,
                    ReplicationFactor = 1
                }
            };

            try
            {
                await adminClient.CreateTopicsAsync(topics);
                logger.LogInformation("Topicos creados exitosamente: {Topics}",
                    string.Join(", ", topics.Select(t => t.Name)));
            }
            catch (CreateTopicsException ex)
            {
                foreach (var result in ex.Results)
                {
                    if (result.Error.Code == ErrorCode.TopicAlreadyExists)
                    {
                        logger.LogInformation(ex, "El topico {TopicName} ya existe en el broker", result.Topic);
                    }
                    else if (result.Error.Code != ErrorCode.NoError)
                    {
                        logger.LogError(ex, "Fallo la creación del topico {TopicName}: {Error}",
                            result.Topic, result.Error.Reason);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creando topicos en broker. BootstrapServers: {BootstrapServers}, Topics: {Topics}",
                    kafkaOptions.Value.BootstrapServers,
                    string.Join(", ", topics.Select(t => t.Name)));
            }
        }
    }
}