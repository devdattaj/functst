using Altera.Ingestion.Core.HttpClientHandlers;
using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Mappers;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Core.Options;
using Altera.Ingestion.Core.Repositories;
using Altera.Ingestion.Core.Services;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Hl7.Fhir.Serialization;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestCorrelationServices(this IServiceCollection services)
    {
        services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

        return services;
    }

    public static IServiceCollection AddFhirApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var fhirServiceOptions = configuration.GetRequiredSection(FhirServiceOptions.ConfigurationSectionName).Get<FhirServiceOptions>();

        services.AddOptions<FhirServiceOptions>().Bind(configuration.GetSection(FhirServiceOptions.ConfigurationSectionName));

        services.AddTransient<FhirJsonParser>();
        services.AddTransient<FhirJsonSerializer>();

        services.AddTransient<IFhirApiAuthenticationTokenService, FhirApiAuthenticationTokenService>();
        services.AddTransient<AuthenticationDelegatingHandler>();
        services
            .AddHttpClient<IFhirApiService, FhirApiService>(client =>
            {
                client.BaseAddress = new Uri(fhirServiceOptions.ServiceBaseUri);
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return services;
    }

    public static IServiceCollection AddMapperServices(this IServiceCollection services)
    {
        services.AddTransient<IMapper<IngestionInitiatedEvent, Job>, IngestionInitiatedEventToJobMapper>();
        services.AddTransient<IMapper<LoadCompleteEvent, Batch>, LoadCompleteEventToBatchMapper>();
        services.AddTransient<IMapper<ValidationCompleteEvent, BatchEvent>, ValidationCompleteEventToBatchEventMapper>();
        services.AddTransient<IMapper<SecurityCompleteEvent, BatchEvent>, SecurityCompleteEventToBatchEventMapper>();
        services.AddTransient<IMapper<TransformationCompleteEvent, BatchEvent>, TransformationCompleteEventToBatchEventMapper>();
        services.AddTransient<IMapper<AggregateImportCompleteEvent, BatchEvent>, AggregateImportCompleteEventToBatchEventMapper>();
        services.AddTransient<IMapper<ImportCompleteEvent, Job>, ImportCompleteEventToJobMapper>();

        services.AddTransient<IMapper<Job, JobData>, JobToJobDataMapper>();
        services.AddTransient<IMapper<Batch, BatchData>, BatchToBatchDataMapper>();
        services.AddTransient<IMapper<BatchEvent, BatchEventData>, BatchEventToBatchEventDataMapper>();

        services.AddTransient<IMapperService, MapperService>();

        return services;
    }

    public static IServiceCollection AddServiceBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceBusOptions = configuration.GetRequiredSection(ServiceBusOptions.ConfigurationSectionName).Get<ServiceBusOptions>();

        services.AddOptions<ServiceBusOptions>().Bind(configuration.GetSection(ServiceBusOptions.ConfigurationSectionName));

        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddClient<ServiceBusClient, ServiceBusClientOptions>((_, _, _) =>
            {
                var options = new ServiceBusClientOptions
                {
                    RetryOptions = new ServiceBusRetryOptions
                    {
                        Delay = TimeSpan.FromSeconds(serviceBusOptions.RetryDelaySeconds),
                        MaxDelay = TimeSpan.FromSeconds(serviceBusOptions.MaxRetryDelaySeconds),
                        MaxRetries = serviceBusOptions.MaxRetries,
                        Mode = (ServiceBusRetryMode)serviceBusOptions.RetryMode,
                        TryTimeout = TimeSpan.FromSeconds(serviceBusOptions.RetryTryTimeoutSeconds)
                    }
                };

                return new ServiceBusClient(serviceBusOptions.Namespace, new DefaultAzureCredential(), options);
            });

            foreach (var (topic, topicName) in serviceBusOptions.OutboundTopicNamesMap)
            {
                clientBuilder
                    .AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, serviceProvider) =>
                        serviceProvider.GetRequiredService<ServiceBusClient>().CreateSender(topicName))
                    .WithName(topic);
            }
        });

        services.AddTransient<IServiceBusSenderService, ServiceBusSenderService>();

        return services;
    }

    public static IServiceCollection AddSynchronizationDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var cosmosDbOptions = configuration.GetRequiredSection(CosmosDbOptions.ConfigurationSectionName).Get<CosmosDbOptions>();

        services.AddTransient<ICosmosLinqQuery, CosmosLinqQuery>();

        services.AddOptions<CosmosDbOptions>().Bind(configuration.GetSection(CosmosDbOptions.ConfigurationSectionName));

        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddClient<CosmosClient, CosmosClientOptions>((_, _, _) =>
                new CosmosClient(cosmosDbOptions.Endpoint, new DefaultAzureCredential(), new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                }));
        });

        services.AddTransient<IJobRepository, JobRepository>((serviceProvider) =>
        {
            var client = serviceProvider.GetRequiredService<CosmosClient>();
            var database = client.GetDatabase(cosmosDbOptions.DatabaseId);
            var container = database.GetContainer(cosmosDbOptions.JobsContainerId);

            return new JobRepository(container);
        });

        services.AddTransient<IBatchRepository, BatchRepository>((serviceProvider) =>
        {
            var client = serviceProvider.GetRequiredService<CosmosClient>();
            var database = client.GetDatabase(cosmosDbOptions.DatabaseId);
            var container = database.GetContainer(cosmosDbOptions.BatchesContainerId);
            var cosmosLinqQuery = serviceProvider.GetRequiredService<ICosmosLinqQuery>();

            return new BatchRepository(container, cosmosLinqQuery);
        });

        services.AddTransient<IBatchEventRepository, BatchEventRepository>((serviceProvider) =>
        {
            var client = serviceProvider.GetRequiredService<CosmosClient>();
            var database = client.GetDatabase(cosmosDbOptions.DatabaseId);
            var container = database.GetContainer(cosmosDbOptions.BatchEventsContainerId);

            return new BatchEventRepository(container);
        });

        return services;
    }
}
