using Altera.Ingestion.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Altera.Ingestion.Orchestration.Function;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("local.settings.json", optional: true);

                var keyVaultUri = new Uri(Environment.GetEnvironmentVariable("KeyVaultUri"));
                builder.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
            })
            .ConfigureFunctionsWorkerDefaults((context, builder) =>
            {
                var configuration = context.Configuration;
                var services = builder.Services;

                builder.AddLogEnrichment();
                builder.AddServiceBusMiddleware();
                services.AddMapperServices();
                services.AddRequestCorrelationServices();
                services.AddServiceBusServices(configuration);
                services.AddSynchronizationDatabaseServices(configuration);
                services.AddFunctionServices();
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.AddCoreLogger(context.Configuration);
            })
            .Build();

        host.Run();
    }
}

