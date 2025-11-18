using Arkano.Transactions.Domain.Services;
using MediatR;

namespace Arkano.Transactions.Aplication.Transactions.Commands
{
    public class UpdateTransactionStatusCommandHandler(TransactionStatusService transactionStatusService) : IRequestHandler<UpdateTransactionStatusCommand>
    {
        public async Task Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            await transactionStatusService.Update(request.TransactionExternalId, request.Status, cancellationToken);
        }
    }
}
