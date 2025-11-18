namespace Arkano.Transactions.Domain.Ports
{
    public interface IEvent
    {
        string Subject { get; }
    }
}
