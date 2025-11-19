using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Arkano.Transactions.Infraestructure.Data
{
    public class DesignTimeTransactionsDbContextFactory : IDesignTimeDbContextFactory<TransactionsDbContext>
    {
        public TransactionsDbContext CreateDbContext(string[] args)
        {            
            var connectionFromEnv = Environment.GetEnvironmentVariable("EF_CONNECTION");
            string connectionString;

            if (!string.IsNullOrEmpty(connectionFromEnv))
            {
                connectionString = connectionFromEnv;
            }
            else
            {
                connectionString = "Host=localhost;Port=5433;Database=transactiondb;Username=postgres;Password=postgres";
            }

            var optionsBuilder = new DbContextOptionsBuilder<TransactionsDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TransactionsDbContext(optionsBuilder.Options);
        }
    }
}
