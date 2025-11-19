using Arkano.Transactions.Aplication.Transactions.Commands;

namespace Arkano.Transactions.Domain.Tests.Builders
{
    public class CreateTransactionCommandBuilder
    {
        private Guid _sourceAccountId = Guid.NewGuid();
        private Guid _targetAccountId = Guid.NewGuid();
        private int _value = 1;
        private decimal _amount = 100m;

        public static CreateTransactionCommandBuilder Create() => new();

        public CreateTransactionCommandBuilder WithSourceAccountId(Guid sourceAccountId)
        {
            _sourceAccountId = sourceAccountId;
            return this;
        }

        public CreateTransactionCommandBuilder WithTargetAccountId(Guid targetAccountId)
        {
            _targetAccountId = targetAccountId;
            return this;
        }

        public CreateTransactionCommandBuilder WithValue(int value)
        {
            _value = value;
            return this;
        }

        public CreateTransactionCommandBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public CreateTransactionCommandBuilder WithEmptySourceAccount()
        {
            _sourceAccountId = Guid.Empty;
            return this;
        }

        public CreateTransactionCommandBuilder WithEmptyTargetAccount()
        {
            _targetAccountId = Guid.Empty;
            return this;
        }

        public CreateTransactionCommandBuilder WithLargeAmount()
        {
            _amount = 50000m;
            return this;
        }

        public CreateTransactionCommandBuilder WithSmallAmount()
        {
            _amount = 0.01m;
            return this;
        }

        public CreateTransactionCommandBuilder WithZeroAmount()
        {
            _amount = 0m;
            return this;
        }

        public CreateTransactionCommandBuilder WithNegativeAmount()
        {
            _amount = -100m;
            return this;
        }

        public CreateTransactionCommand Build()
        {
            return new CreateTransactionCommand(_sourceAccountId, _targetAccountId, _value, _amount);
        }
    }
}