using Arkano.Transactions.Domain.Entities;
using Arkano.Transactions.Infraestructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Arkano.Transactions.Infraestructure.Data
{
    public class TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());           

            base.OnModelCreating(modelBuilder);
        }
    }
}
