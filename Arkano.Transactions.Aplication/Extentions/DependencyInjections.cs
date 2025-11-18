using Arkano.Transactions.Aplication.Attributes;
using Arkano.Transactions.Aplication.Fabrics;
using Arkano.Transactions.Domain.Fabrics;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Arkano.Transactions.Aplication.Extentions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddAplicationApi(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjections).Assembly);
                configuration.TypeEvaluator = WithAttribute<ManagedApiAttribute>();
            });

            return services;
        }

        public static IServiceCollection AddFabrics(this IServiceCollection services)
        {
            services.AddTransient<ITransactionFactory, TransactionFactory>();
            services.AddTransient<IResultFactory, ResultFactory>();
            services.AddTransient<IAntifraudResultFabric, AntifraudResultFabric>();
            services.AddTransient<ICheckTransactionStateFactory, CheckTransactionStateFactory>();

            return services;
        }

        public static Func<Type, bool> WithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return type => type.GetCustomAttributes<TAttribute>() is not null;
        }
    }
}
