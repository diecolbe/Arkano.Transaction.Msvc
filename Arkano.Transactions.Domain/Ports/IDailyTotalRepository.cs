namespace Arkano.Transactions.Domain.Ports
{
    public interface IDailyTotalRepository
    {
        Task<decimal> GetDailyTotalAmountAsync(Guid accountExternalId, CancellationToken cancellationToken = default);
    }
}
