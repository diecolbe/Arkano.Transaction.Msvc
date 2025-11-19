using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Infraestructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Arkano.Transactions.Infraestructure.Data
{
    public class TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                value => value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }

            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ConvertDateTimesToUtc();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDateTimesToUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDateTimesToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added || entity.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue is DateTime dateTime)
                    {
                        if (dateTime.Kind == DateTimeKind.Unspecified)
                        {
                            property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                        }
                        else if (dateTime.Kind == DateTimeKind.Local)
                        {
                            property.CurrentValue = dateTime.ToUniversalTime();
                        }
                    }
                }
            }
        }
    }
}
