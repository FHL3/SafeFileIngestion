public record IngestionResultRecord
{
//File name.
// Number of records processed.
// Whether ingestion succeeded.
    public string? FileName { get; init; }
    public int NumberOfRecordsProcessed { get; init; }
    public bool IngestionsSucceeded { get; init; }
}