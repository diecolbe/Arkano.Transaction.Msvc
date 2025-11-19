using Arkano.Transactions.Aplication.Common;
using Arkano.Transactions.Aplication.Constants;
using Arkano.Transactions.Aplication.Dtos;
using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Enums;
using Arkano.Transactions.Domain.Services;
using MediatR;

namespace Arkano.Transactions.Aplication.Transactions.Queries
{
    public class TransactionQueryHandler(IResultFactory resultFactory, ICheckTransactionStateFactory checkFactory, TransactionService transactionService)
        : IRequestHandler<TransactionQuery, ResultRequest<CheckTransactionStateDto>>
    {
        private readonly IResultFactory _resultFactory = resultFactory;
        private readonly ICheckTransactionStateFactory _checkFactory = checkFactory;
        private readonly TransactionService _transactionService = transactionService;

        public async Task<ResultRequest<CheckTransactionStateDto>> Handle(TransactionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                TransactionStatus status = await _transactionService.GetTransactionStatusByIdAsync(request.TransactionExternalId, cancellationToken);

                CheckTransactionStateDto checkTransactionStateDto = _checkFactory.FromStatus(status);

                return _resultFactory.Success(checkTransactionStateDto);
            }
            catch (ArgumentException)
            {
                return _resultFactory.Fail<CheckTransactionStateDto>(MessageConstants.InvalidArgumentErrorMessage);
            }
            catch (Exception)
            {
                return _resultFactory.Fail<CheckTransactionStateDto>(MessageConstants.UnexpectedError, (int)System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
