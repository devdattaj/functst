namespace Altera.Ingestion.Import.Function.Options;

public record ImportOptions
{
    public const string ConfigurationSectionName = "Import";

    public int ImportTimeoutSeconds { get; init; }

    public int ImportPollPeriodSeconds { get; init; }
}
