namespace Arkano.Transactions.Domain.Dtos
{
    public record ValidationDataDto
    {
        public Guid TransactionExternalId { get; set; }
        public bool IsValid { get; set; }
        public string ValidationReason { get; set; } = string.Empty;        
        public DateTime ProcessedAt { get; set; }      
        public string Status { get; set; } = string.Empty;
    }
}
