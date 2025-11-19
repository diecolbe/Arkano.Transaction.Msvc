using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Domain.Tests.Builders
{
    public class AccountBuilder
    {
        private Guid _accountId = Guid.NewGuid();
        private string _ownerName = "Default Owner";

        public static AccountBuilder Create() => new();

        public AccountBuilder WithAccountId(Guid accountId)
        {
            _accountId = accountId;
            return this;
        }

        public AccountBuilder WithOwnerName(string ownerName)
        {
            _ownerName = ownerName;
            return this;
        }

        public AccountBuilder WithEmptyAccountId()
        {
            _accountId = Guid.Empty;
            return this;
        }

        public AccountBuilder WithNullOwnerName()
        {
            _ownerName = null!;
            return this;
        }

        public AccountBuilder WithEmptyOwnerName()
        {
            _ownerName = string.Empty;
            return this;
        }

        public AccountBuilder WithLongOwnerName()
        {
            _ownerName = new string('A', 300);
            return this;
        }

        public AccountBuilder WithSpecialCharactersInName()
        {
            _ownerName = "John @#$%^&*() Doe";
            return this;
        }

        public Account Build()
        {
            return new Account(_accountId, _ownerName);
        }

        public static IEnumerable<Account> CreateMultiple(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Create()
                    .WithAccountId(Guid.NewGuid())
                    .WithOwnerName($"Owner {i + 1}")
                    .Build();
            }
        }
    }
}