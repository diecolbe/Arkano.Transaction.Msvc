using Arkano.Transactions.Domain.Enums;

namespace Arkano.Transactions.Domain.Services
{
    public static class ValidateStatusService
    {
        public static string Status(TransactionStatus status)
        {
            return status switch
            {
                TransactionStatus.Approved => "La transacción fue Aprobada",
                TransactionStatus.Rejected => "La transacción fue Rechazada",
                TransactionStatus.Pending => "La transacción esta pendiente de validación",
                _ => throw new ArgumentException("Estado de transacción inválido.", nameof(status))
            };
        }
    }
}
