using Microsoft.Azure.Cosmos;

namespace Altera.Ingestion.Core.Interfaces;

public interface ICosmosLinqQuery
{
    FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query);
}
