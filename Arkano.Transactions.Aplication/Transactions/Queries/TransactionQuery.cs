using Arkano.Transactions.Aplication.Common;
using Arkano.Transactions.Aplication.Dtos;
using MediatR;

namespace Arkano.Transactions.Aplication.Transactions.Queries
{
    public record TransactionQuery(
        Guid TransactionExternalId,
        DateTime CreatedAt)
        : IRequest<ResultRequest<CheckTransactionStateDto>>;
}
