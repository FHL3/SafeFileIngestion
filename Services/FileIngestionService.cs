public class FileIngestionService
{
    //FileIngestionService is responsible for reading and processing files
    // The method IngestFile accepts a file path as a parameter.
    // It will return an IngestionResultRecord object that contains the file name, number of records processed, and whether the ingestion succeeded.

    //returning IngestFile - thrIt receives a file path, processes the file, and returns a record describing the outcome.
    public IngestionResultRecord IngestFile(string filePath) //this returns an IngestionResultRecord object that contains the file name, number of records processed, and whether the ingestion succeeded.
    {

        if (File.Exists(filePath))
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read); // Open the file for reading. - Construct a FileStream object to read the file.
            // Read the contents of the file and return it.
            // Removed return File.ReadLines(filePath).ToList(); becuase it was not reading the file line by line and processing each line.
            // Instead, it was reading the entire file at once and returning it as a list of strings.
            // Using FileStream and StreamReader to read and process the file progressively, one line at a time.
            using StreamReader reader = new StreamReader(fileStream); // Create a StreamReader to read the file line by line - opens the file for reading. We can process each line and move on without retaining the entire file's contents.
            
            int numberOfRecordsProcessed = 0; // Initialize a counter for the number of records processed.
            string? line;
            
            while ((line = reader.ReadLine()) != null) // Read each line of the file until the end is reached.
            {
                // Process each line here. For now, we will just increment the counter.
                numberOfRecordsProcessed++;
            }
           
           return new IngestionResultRecord
            {
                FileName = filePath,
                NumberOfRecordsProcessed = numberOfRecordsProcessed,
                IngestionsSucceeded = true
            };
        }
        else
        {
            throw new FileNotFoundException("The specified file does not exist."); // - Throw an exception if the file does not exist. Throw is used to indicate that an error has occurred and the method cannot continue. It will be caught in the calling code.
        }
    }
}