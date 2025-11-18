using Arkano.Transactions.Aplication.Extentions;
using Arkano.Transactions.Domain.Models;
using Arkano.Transactions.Infraestructure.Extentions;
using Arkano.Transactions.Worker.Workers;
using System.Reflection;

namespace Arkano.Transactions.Worker.Extentions
{
    public static class DependencyInjections
    {
        public static void AddWorkerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.Load("Arkano.Transactions.Aplication")));

            services.AddAplicationApi();
            services.AddFabrics();

            services.Configure<AntifraudOptions>(configuration.GetSection(AntifraudOptions.SectionName));           

            services.AddInfraestructure(configuration);

            services.AddKafkaServices(configuration);
            services.AddHostedService<TransactionValidatedWorker>();
        }
    }
}
