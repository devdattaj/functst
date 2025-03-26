using Altera.Ingestion.Load.Function.Interfaces;
using Altera.Ingestion.Load.Function.Options;
using Altera.Ingestion.Load.Function.Repositories;
using Altera.Ingestion.Load.Function.Services;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Load.Function;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFunctionServices(this IServiceCollection services, IConfiguration configuration)
    {
        var landingStorageOptions = configuration.GetRequiredSection(LandingStorageOptions.ConfigurationSectionName).Get<LandingStorageOptions>();

        services.AddOptions<LandingStorageOptions>().Bind(configuration.GetSection(LandingStorageOptions.ConfigurationSectionName));

        services.AddTransient<ILoadService, LoadService>();
        services.AddTransient<ILandingRepository, LandingRepository>();

        services.AddAzureClients(clientBuiler =>
        {
            var credential = new DefaultAzureCredential();

            clientBuiler.UseCredential(credential);

            clientBuiler.AddBlobServiceClient(new Uri(landingStorageOptions.LandingBlobStorageUri));
        });

        return services;
    }
}
