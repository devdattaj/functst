using Altera.Ingestion.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Altera.Ingestion.Core.Services;

public class CosmosLinqQuery : ICosmosLinqQuery
{
    public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query) => query.ToFeedIterator();
}
