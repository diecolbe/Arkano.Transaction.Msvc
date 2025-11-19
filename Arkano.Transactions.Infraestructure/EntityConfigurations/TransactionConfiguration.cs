using Arkano.Transactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arkano.Transactions.Infraestructure.EntityConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(transaction => transaction.TransactionExternalId);

            builder.Property(transaction => transaction.TransactionExternalId)
                .ValueGeneratedNever();

            builder.Property(transaction => transaction.SourceAccountId)
                .IsRequired();

            builder.Property(transaction => transaction.TargetAccountId)
                .IsRequired();

            builder.Property(transaction => transaction.Value)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(transaction => transaction.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(transaction => transaction.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        }
    }
}
