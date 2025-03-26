using Altera.Ingestion.Integration.Messages;

namespace Altera.Ingestion.Load.Function.Interfaces;

public interface ILandingRepository
{
    Task<IEnumerable<FileMetadata>> CopyFromAsync(string[] filePaths, CancellationToken cancelationToken);
}
