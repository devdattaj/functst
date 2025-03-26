namespace Altera.Ingestion.Core.Options;

public record ServiceBusOptions
{
    public const string ConfigurationSectionName = "ServiceBus";

    public string Namespace { get; init; }

    public Dictionary<string, string> OutboundTopicNamesMap { get; init; }

    public int RetryDelaySeconds { get; init; }

    public int MaxRetryDelaySeconds { get; init; }

    public int MaxRetries { get; init; }

    public int RetryMode { get; init; }

    public int RetryTryTimeoutSeconds { get; init; }
}
