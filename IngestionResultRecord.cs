public record IngestionResultRecord
{
    public string? FileName { get; init; }
    public int NumberOfRecordsProcessed { get; init; }
    public bool IngestionsSucceeded { get; init; }
}