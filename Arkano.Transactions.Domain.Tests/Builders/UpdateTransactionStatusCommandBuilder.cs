using Arkano.Transactions.Aplication.Transactions.Commands;

namespace Arkano.Transactions.Domain.Tests.Builders
{
    public class UpdateTransactionStatusCommandBuilder
    {
        private Guid _transactionExternalId = Guid.NewGuid();
        private string _status = "Approved";

        public static UpdateTransactionStatusCommandBuilder Create() => new();

        public UpdateTransactionStatusCommandBuilder WithTransactionExternalId(Guid transactionExternalId)
        {
            _transactionExternalId = transactionExternalId;
            return this;
        }

        public UpdateTransactionStatusCommandBuilder WithStatus(string status)
        {
            _status = status;
            return this;
        }

        public UpdateTransactionStatusCommandBuilder WithApprovedStatus()
        {
            _status = "Approved";
            return this;
        }

        public UpdateTransactionStatusCommandBuilder WithRejectedStatus()
        {
            _status = "Rejected";
            return this;
        }

        public UpdateTransactionStatusCommandBuilder WithInvalidStatus()
        {
            _status = "InvalidStatus";
            return this;
        }

        public UpdateTransactionStatusCommandBuilder WithEmptyTransactionId()
        {
            _transactionExternalId = Guid.Empty;
            return this;
        }

        public UpdateTransactionStatusCommand Build()
        {
            return new UpdateTransactionStatusCommand(_transactionExternalId, _status);
        }
    }
}
