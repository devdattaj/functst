using Altera.Ingestion.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Identity;

namespace Altera.Ingestion.Load.Function;

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
                services.AddRequestCorrelationServices();
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
