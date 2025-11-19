namespace Arkano.Transactions.Domain.Dtos
{
    public record TransactionValidatedDto(
        Guid TransactionExternalId,
        bool IsValid,
        string ValidationReason,
        DateTime ProcessedAt,
        string Status);
}
