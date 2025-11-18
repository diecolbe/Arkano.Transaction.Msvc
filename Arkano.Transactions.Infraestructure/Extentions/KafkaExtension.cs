using Arkano.Transactions.Domain.Models;
using Arkano.Transactions.Domain.Ports;
using Arkano.Transactions.Infraestructure.Adapters;
using Arkano.Transactions.Infraestructure.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Arkano.Transactions.Infraestructure.Extentions
{
    public static class KafkaExtension
    {
        public static void AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>(options => configuration.GetSection(KafkaOptions.SectionName).Bind(options));

            services.AddSingleton<IProducer<Null, string>>(serviceProvider =>
            {
                var kafkaOptions = serviceProvider.GetRequiredService<IOptions<KafkaOptions>>().Value;

                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = kafkaOptions.BootstrapServers,
                    MessageTimeoutMs = kafkaOptions.MessageTimeoutMs,
                    RetryBackoffMs = kafkaOptions.RetryBackoffMs,
                    EnableIdempotence = kafkaOptions.EnableIdempotence,
                    Acks = Acks.All
                };

                return new ProducerBuilder<Null, string>(producerConfig)
                    .SetErrorHandler((_, e) =>
                    {
                        Console.WriteLine($"Kafka Producer Error: {e.Reason}");
                    })
                    .Build();
            });

            services.AddSingleton<IEventBus, KafkaServiceBus>();
            services.AddScoped<KafkaTopicManager>();
        }
    }
}
