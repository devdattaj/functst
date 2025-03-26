namespace Altera.Ingestion.Load.Function.Interfaces;

public interface ILoadService
{
    Task LoadFilesAsync(string[] filePaths, string jobId, CancellationToken cancellationToken);
}
