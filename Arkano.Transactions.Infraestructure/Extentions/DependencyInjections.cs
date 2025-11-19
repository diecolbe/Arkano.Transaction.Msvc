using Ardalis.Specification;
using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Attributes;
using Arkano.Transactions.Domain.Ports;
using Arkano.Transactions.Infraestructure.Adapters;
using Arkano.Transactions.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arkano.Transactions.Infraestructure.Extentions
{
    public static class DependencyInjections
    {
        public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDomainServices()
                .AddPersistence(configuration)
                .AddKafkaServices(configuration);
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TransactionsDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<ITransactionRepository, TransactionRepository>();

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            var domainAssembly = typeof(DomainServiceAttribute).Assembly;

            var domainServiceTypes = domainAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(DomainServiceAttribute), inherit: false).Length != 0);

            foreach (var serviceType in domainServiceTypes)
            {
                services.AddTransient(serviceType);
            }

            return services;
        }
    }
}
