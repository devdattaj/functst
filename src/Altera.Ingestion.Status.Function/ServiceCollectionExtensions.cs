using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Status.Function.Interfaces;
using Altera.Ingestion.Status.Function.Mappers;
using Altera.Ingestion.Status.Function.Models;
using Altera.Ingestion.Status.Function.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Status.Function;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStatusFunctionServices(this IServiceCollection services)
    {
        services.AddTransient<IStatusService, StatusService>();

        services.AddTransient<IMapper<JobData, JobStatusResponse>, JobDataToJobStatusResponseMapper>();

        return services;
    }
}
