namespace Arkano.Transactions.Domain.Dtos
{
    public record TransactionCreatedDto(Guid TransactionExternalId, decimal Value, string Status, decimal TotalValueDaily);
}
