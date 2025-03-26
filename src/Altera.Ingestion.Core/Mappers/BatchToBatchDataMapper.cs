using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Domain.Models;

namespace Altera.Ingestion.Core.Mappers;

public class BatchToBatchDataMapper : IMapper<Batch, BatchData>
{
    public BatchData Map(Batch source) => new BatchData
    {
        Id = source.Id,
        JobId = source.JobId,
        FileName = source.FileName,
        RawFileName = source.RawFileName
    };
}
