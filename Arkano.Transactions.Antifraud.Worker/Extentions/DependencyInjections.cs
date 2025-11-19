using Arkano.Transactions.Antifraud.Worker.Workers;
using Arkano.Transactions.Domain.Fabrics;
using Arkano.Transactions.Domain.Models;
using Arkano.Transactions.Domain.Services;
using Arkano.Transactions.Infraestructure.Extentions;

namespace Arkano.Transactions.Antifraud.Worker.Extentions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddWorkerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AntifraudOptions>(configuration.GetSection(AntifraudOptions.SectionName));           

            services.AddSingleton<AntifraudValidationService>();

            services.AddSingleton<IAntifraudResultFabric, AntifraudResultFabric>();
            services.AddSingleton<ITransactionValidatedDtoFabric, TransactionValidatedDtoFabric>();

            services.AddKafkaServices(configuration);
            services.AddHostedService<AntifraudWorker>();

            return services;
        }
    }
}
