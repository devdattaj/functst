namespace Altera.Ingestion.Orchestration.Function.Interfaces;

public interface IStrategyFactory
{
    IOrchestrationStrategy GetStrategy(string type);
}
