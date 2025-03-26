using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Altera.Ingestion.Core;

public static class LoggingBuilderExtensions
{
    public static void AddCoreLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        loggingBuilder.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        loggingBuilder.AddSerilog();
    }
}
