namespace Arkano.Transactions.Domain.Entities
{
    public class Account(Guid accountId, string ownerName)
    {
        public Guid AccountId { get; private set; } = accountId;

        public string OwnerName { get; private set; } = ownerName;
    }
}
