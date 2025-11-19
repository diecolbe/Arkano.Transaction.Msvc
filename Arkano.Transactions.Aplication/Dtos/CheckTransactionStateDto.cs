using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Aplication.Dtos
{
    public record CheckTransactionStateDto(TransactionStatus State);
}
