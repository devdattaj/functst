namespace Altera.Ingestion.Core.Exceptions;

public class FhirApiException : Exception
{
    public FhirApiException()
    {
    }

    public FhirApiException(string message) : base(message)
    {
    }

    public FhirApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
