using System;
using System.IO;
Console.WriteLine("First steps in File Input/Output in C#");

List<string> files = new List<string> { "Fruits.txt", "Birds.txt", "trucks.txt" }; 
// Declared a list of file names to be processed. - This list contains the names of the files that will be processed by the FileIngestionService.

// Create an instance of the FileIngestionService
FileIngestionService fileIngestionService = new FileIngestionService();

int successfulIngestions = 0; // Initialize a counter for successful ingestions.
int failedIngestions = 0; // Initialize a counter for failed ingestions.
int totalRecordsProcessed = 0; // Initialize a counter for the total number of records processed.

foreach (string file in files) // - Iterates through the list of files to be processed. Paasess each file name to the IngestFile method of the FileIngestionService class for processing.
{
    try
    {
        IngestionResultRecord result = fileIngestionService.IngestFile(file); // - Calls the IngestFile method of the FileIngestionService class to process the file and returns an IngestionResultRecord object.
        successfulIngestions++; // - Increases the counter for successful ingestions by 1 if the ingestion succeeded.
        totalRecordsProcessed += result.NumberOfRecordsProcessed; // - Increment the counter for the total number of records processed by adding the number of records processed in the current file to the total.
        Console.WriteLine($"File: {result.FileName}, Records Processed: {result.NumberOfRecordsProcessed}, Ingestion Succeeded: {result.IngestionsSucceeded}"); // - Displays the file name, number of records processed, and whether the ingestion succeeded on the console.
        
    }
    catch (Exception ex) // - Catches any exceptions that may occur during the ingestion process. This is important for handling errors, preventing the application from crashing.
    { //the exception inside LogError() could escape our try-catch block, and terminate our application, so we need to catch it here and log it
      // to the console by nesting another try-catch block inside the catch block of the outer try-catch block. This way, we can handle any exceptions that may occur during logging and prevent them from crashing our application.
        try
        {
        LoggingFile loggingFile = new LoggingFile();
        loggingFile.LogError(ex, "error.log", file); // - Logs the exception details to the specified log file using the LogError method of the LoggingFile class. The log file path is "error.log" and the error location is the current file being processed.
        Console.WriteLine($"An error occurred while processing the file '{file}'. Details have been logged to 'error.log'."); // - Displays an appropriate message on the console.
        // - The LogError method will write the exception message, timestamp, and error location to the log file.
        }
        catch (Exception logEx) // handles a failur while writing to the log file, such as if the log file is locked or the disk is full. This ensures that the application can continue running even if logging fails.
        {
            Console.WriteLine($"Failed to log error for file '{file}'. Exception: {logEx.Message}"); // - Displays an appropriate message on the console if logging fails.
            //Exception: {logEx.Message} exposing internal exception details.
        }

        failedIngestions++; // - Increases the counter for failed ingestions by 1 if an exception occurred during ingestion.
        //FailedIngestions++ belongs outside the inner try-catch, but inside the outer catch. Because the file ingestion has already failed, regardless of whether logging succeeds or fails.
        //If we placed the counter inside either inner branch, we would risk tying the ingestion count to the logging outcome.
        
    }
}

Console.WriteLine($"Total Successful Ingestions: {successfulIngestions}, Total Failed Ingestions: {failedIngestions}, Total Records Processed: {totalRecordsProcessed}"); // - Displays the total number of successful ingestions, failed ingestions, and total records processed on the console.