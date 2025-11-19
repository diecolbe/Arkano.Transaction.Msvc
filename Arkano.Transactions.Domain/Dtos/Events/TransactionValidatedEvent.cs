using Arkano.Transactions.Domain.Ports;

namespace Arkano.Transactions.Domain.Dtos.Events
{
    public record TransactionValidatedEvent(object Data) : IEvent
    {
        public string Subject => nameof(TransactionValidatedEvent);
    }
}
