using Arkano.Transactions.Domain.Ports;

namespace Arkano.Transactions.Domain.Dtos.Events
{
    public record TransactionCreatedEvent(object Data) : IEvent
    {
        public string Subject => nameof(TransactionCreatedEvent);
    }
}
