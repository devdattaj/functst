namespace Altera.Ingestion.Import.Function.Exceptions;

public class ImportCompletionNotReachedException : Exception
{
    public ImportCompletionNotReachedException()
    {
    }

    public ImportCompletionNotReachedException(string message) : base(message)
    {
    }

    public ImportCompletionNotReachedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
