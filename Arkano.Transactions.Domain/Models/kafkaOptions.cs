namespace Arkano.Transactions.Domain.Models
{
    public class KafkaOptions
    {
        public const string SectionName = "Kafka";

        public string BootstrapServers { get; set; } = string.Empty;

        public string GroupId { get; set; } = string.Empty;

        public string TransactionCreatedTopic { get; set; } = string.Empty;

        public string TransactionValidatedTopic { get; set; } = string.Empty;

        public int MessageTimeoutMs { get; set; } = 5000;

        public int RetryBackoffMs { get; set; } = 100;

        public bool EnableIdempotence { get; set; } = true;
    }
}
