namespace Altera.Ingestion.Core.Interfaces;

public interface IMapperService
{
    TDestination Map<TSource, TDestination>(TSource source);
}
