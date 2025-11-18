using System.ComponentModel.DataAnnotations;

namespace Arkano.Transactions.Aplication.Dtos
{
    public record CreateTransactionDto(
        [Required] Guid SourceAccountId,
        [Required] Guid TargetAccountId,
        [Required] int TranferTypeId,
        [Required] decimal Value);
}
