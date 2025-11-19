using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Domain.Services
{
    public static class ValidateStatusService
    {
        public static string Status(TransactionStatus status)
        {
            return status switch
            {
                TransactionStatus.Approved => "Aprobada",
                TransactionStatus.Rejected => "Rechazada",
                TransactionStatus.Pending => "Pendiente",
                _ => throw new ArgumentException("Estado de transacción inválido.", nameof(status))
            };
        }
    }
}
