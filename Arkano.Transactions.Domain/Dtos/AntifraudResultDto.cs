namespace Arkano.Transactions.Domain.Dtos
{
    public record AntifraudResultDto
    {
        public Guid TransactionExternalId { get; set; }
        public decimal TransactionValue { get; set; }
        public bool IsValid { get; set; }
        public string ValidationReason { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
    }
}
