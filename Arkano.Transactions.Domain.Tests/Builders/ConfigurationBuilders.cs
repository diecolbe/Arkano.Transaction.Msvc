using Arkano.Transactions.Domain.Models;
using Microsoft.Extensions.Options;

namespace Arkano.Transactions.Domain.Tests.Builders
{
    public class KafkaOptionsBuilder
    {
        private string _bootstrapServers = "localhost:9092";
        private string _groupId = "test-group";
        private string _transactionCreatedTopic = "transaction-created";
        private string _transactionValidatedTopic = "transaction-validated";
        private int _messageTimeoutMs = 5000;
        private int _retryBackoffMs = 1000;
        private bool _enableIdempotence = true;

        public static KafkaOptionsBuilder Create() => new();

        public KafkaOptionsBuilder WithBootstrapServers(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
            return this;
        }

        public KafkaOptionsBuilder WithGroupId(string groupId)
        {
            _groupId = groupId;
            return this;
        }

        public KafkaOptionsBuilder WithTransactionCreatedTopic(string topic)
        {
            _transactionCreatedTopic = topic;
            return this;
        }

        public KafkaOptionsBuilder WithTransactionValidatedTopic(string topic)
        {
            _transactionValidatedTopic = topic;
            return this;
        }

        public KafkaOptionsBuilder WithMessageTimeoutMs(int timeoutMs)
        {
            _messageTimeoutMs = timeoutMs;
            return this;
        }

        public KafkaOptionsBuilder WithRetryBackoffMs(int backoffMs)
        {
            _retryBackoffMs = backoffMs;
            return this;
        }

        public KafkaOptionsBuilder WithIdempotenceEnabled(bool enabled)
        {
            _enableIdempotence = enabled;
            return this;
        }

        public KafkaOptionsBuilder WithTestEnvironmentDefaults()
        {
            _bootstrapServers = "localhost:9092";
            _messageTimeoutMs = 1000;
            _retryBackoffMs = 100;
            return this;
        }

        public KafkaOptionsBuilder WithProductionDefaults()
        {
            _messageTimeoutMs = 30000;
            _retryBackoffMs = 5000;
            _enableIdempotence = true;
            return this;
        }

        public KafkaOptions Build()
        {
            return new KafkaOptions
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                TransactionCreatedTopic = _transactionCreatedTopic,
                TransactionValidatedTopic = _transactionValidatedTopic,
                MessageTimeoutMs = _messageTimeoutMs,
                RetryBackoffMs = _retryBackoffMs,
                EnableIdempotence = _enableIdempotence
            };
        }

        public IOptions<KafkaOptions> BuildAsOptions()
        {
            return Options.Create(Build());
        }
    }
}