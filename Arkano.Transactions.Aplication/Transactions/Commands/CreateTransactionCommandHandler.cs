using Arkano.Transactions.Aplication.Common;
using Arkano.Transactions.Aplication.Constants;
using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Domain.Services;
using MediatR;

namespace Arkano.Transactions.Aplication.Transactions.Commands
{
    public class CreateTransactionCommandHandler(TransactionService transactionService, ITransactionFactory transactionFactory, IResultFactory resultFactory)
        : IRequestHandler<CreateTransactionCommand, ResultRequest<Guid>>
    {
        public async Task<ResultRequest<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Transaction transaction = transactionFactory.Create(request.SourceAccountId, request.TargetAccountId, request.Value);

                var transactionExternalId = await transactionService.CreateAsync(transaction, cancellationToken);

                return resultFactory.TransactionCreated(transactionExternalId);
            }
            catch (ArgumentException)
            {
                return resultFactory.Fail<Guid>(MessageConstants.InvalidArgumentErrorMessage);
            }
            catch (Exception)
            {
                return resultFactory.Fail<Guid>(MessageConstants.UnexpectedError, (int)System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
