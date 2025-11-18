namespace Arkano.Transactions.Domain.Ports
{
    public interface IEventBus
    {
        Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
    }
}
