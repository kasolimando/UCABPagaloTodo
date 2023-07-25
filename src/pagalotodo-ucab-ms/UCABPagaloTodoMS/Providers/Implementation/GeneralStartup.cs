using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Infrastructure.Settings;

namespace UCABPagaloTodoMS.Providers.Implementation
{
    [ExcludeFromCodeCoverage]
    public static class GeneralStartup
    {
        public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration,
            string versionNumber, AppSettings appSettings, string environment)
        {
            Providers provider = new();
            provider.AddCors(services);


            if (appSettings.RequireControllers)
            {
                provider.AddHealthCheck(services, configuration, appSettings);
                provider.AddControllers(services, configuration, appSettings);
            }

            if (appSettings.RequireSwagger)
            {
                provider.AddSwagger(services, versionNumber, appSettings);
            }

            provider.AddDatabaseService(services, configuration, environment, appSettings.RequireDatabase);

            return services;
        }
    }
}
