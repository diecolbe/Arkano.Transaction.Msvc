using Arkano.Transactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arkano.Transactions.Infraestructure.EntityConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(account => account.AccountId);

            builder.Property(account => account.AccountId)
                .ValueGeneratedNever();

            builder.Property(account => account.OwnerName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
