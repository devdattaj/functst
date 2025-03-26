namespace Altera.Ingestion.Core;

public record ServiceBusTopic
{
    private readonly string _topicKey;

    public static ServiceBusTopic Orchestration => new ServiceBusTopic("OrchestrationTopic");

    public static ServiceBusTopic Load => new ServiceBusTopic("LoadTopic");

    public static ServiceBusTopic Validate => new ServiceBusTopic("ValidateTopic");

    public static ServiceBusTopic Security => new ServiceBusTopic("SecurityTopic");

    public static ServiceBusTopic Transformation => new ServiceBusTopic("TransformationTopic");

    public static ServiceBusTopic Enrichment => new ServiceBusTopic("EnrichmentTopic");

    public static ServiceBusTopic AggregateImport => new ServiceBusTopic("AggregateImportTopic");

    public static ServiceBusTopic InitiateImport => new ServiceBusTopic("InitiateImportTopic");

    public static ServiceBusTopic CompleteImport => new ServiceBusTopic("CompleteImportTopic");

    public static ServiceBusTopic TransformImport => new ServiceBusTopic("TransformImportTopic");

    private ServiceBusTopic(string topicKey)
    {
        _topicKey = topicKey;
    }

    public static implicit operator string(ServiceBusTopic topic) => topic.ToString();

    public override string ToString() => _topicKey;
}
