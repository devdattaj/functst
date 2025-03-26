using Altera.Ingestion.Integration.Messages;

namespace Altera.Ingestion.Import.Function.Interfaces;

public interface IImportService
{
    Task ImportFilesAsync(InitiateImportMessage initiateImportMessage, CancellationToken cancellationToken);
}
