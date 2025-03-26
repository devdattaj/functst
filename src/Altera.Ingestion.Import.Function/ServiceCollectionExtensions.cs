using Altera.Ingestion.Import.Function.Interfaces;
using Altera.Ingestion.Import.Function.Options;
using Altera.Ingestion.Import.Function.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Import.Function;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFunctionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ImportOptions>().Bind(configuration.GetRequiredSection(ImportOptions.ConfigurationSectionName));

        services.AddTransient<IImportService, ImportService>();

        return services;
    }
}
