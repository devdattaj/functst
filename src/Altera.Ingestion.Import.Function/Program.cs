using Altera.Ingestion.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Altera.Ingestion.Import.Function;

public class Program
{
    public static void Main()
    {
        // See the hosting set up at https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide
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
                services.AddRequestCorrelationServices();
                services.AddFhirApiServices(configuration);
                services.AddServiceBusServices(configuration);
                services.AddFunctionServices(configuration);
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.AddCoreLogger(context.Configuration);
            })
            .Build();

        host.Run();
    }
}
