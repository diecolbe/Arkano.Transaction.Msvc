using MediatR;

namespace Arkano.Transactions.Aplication.Transactions.Commands
{
    public record UpdateTransactionStatusCommand(Guid TransactionExternalId, string Status) : IRequest;
}
