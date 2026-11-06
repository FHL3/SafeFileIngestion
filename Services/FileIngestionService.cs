public class FileIngestionService
{

    public IngestionResultRecord IngestFile(string filePath) 
    {

        if (File.Exists(filePath))
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read); 
            using StreamReader reader = new StreamReader(fileStream); 
            
            int numberOfRecordsProcessed = 0; 
            string? line;
            
            while ((line = reader.ReadLine()) != null) 
            {
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
            throw new FileNotFoundException("The specified file does not exist."); 
        }
    }
}