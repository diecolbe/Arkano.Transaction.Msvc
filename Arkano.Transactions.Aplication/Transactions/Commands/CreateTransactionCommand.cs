using Arkano.Transactions.Aplication.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Arkano.Transactions.Aplication.Transactions.Commands
{
    public record CreateTransactionCommand(
        [Required] Guid SourceAccountId,
        [Required] Guid TargetAccountId,
        [Required] int TranferTypeId,
        [Required] decimal Value)
        : IRequest<ResultRequest<Guid>>;
}
