using Arkano.Transactions.Domain.Attributes;
using Arkano.Transactions.Domain.Dtos;
using Arkano.Transactions.Domain.Fabrics;
using Arkano.Transactions.Domain.Models;
using Microsoft.Extensions.Options;

namespace Arkano.Transactions.Domain.Services
{
    [DomainService]
    public class AntifraudValidationService(IOptions<AntifraudOptions> options, IAntifraudResultFabric antifraudResultFabric)
    {
        public AntifraudResultDto ValidateTransaction(TransactionCreatedDto transaction)
        {
            var result = antifraudResultFabric.Create(transaction);

            if (transaction.Value > options.Value.MaxTransactionValue)
            {
                result.IsValid = false;
                result.ValidationReason = $"El valor de la transacción {transaction.Value:C} excede el máximo permitido de {options.Value.MaxTransactionValue:C}";
                return result;
            }

            var projectedAccumulation = transaction.TotalValueDaily + transaction.Value;

            if (projectedAccumulation > options.Value.MaxDailyAccumulation)
            {
                result.IsValid = false;
                result.ValidationReason = $"El acumulado diario sería de {projectedAccumulation:C}, excediendo el máximo permitido de {options.Value.MaxDailyAccumulation:C}";
                return result;
            }

            result.IsValid = true;
            result.ValidationReason = "La transacción ha pasado todas las validaciones antifraude";

            return result;
        }
    }
}