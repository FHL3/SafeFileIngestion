using System;
using System.IO;
Console.WriteLine("First steps in File Input/Output in C#");

List<string> files = new List<string> { "Fruits.txt", "Birds.txt", "trucks.txt" }; 

FileIngestionService fileIngestionService = new FileIngestionService();

int successfulIngestions = 0; 
int failedIngestions = 0; 
int totalRecordsProcessed = 0; 

foreach (string file in files) 
{
    try
    {
        IngestionResultRecord result = fileIngestionService.IngestFile(file); 
        successfulIngestions++; 
        totalRecordsProcessed += result.NumberOfRecordsProcessed; 
        Console.WriteLine($"File: {result.FileName}, Records Processed: {result.NumberOfRecordsProcessed}, Ingestion Succeeded: {result.IngestionsSucceeded}"); 
        
    }
    catch (Exception ex) 
    { 
        try
        {
        LoggingFile loggingFile = new LoggingFile();
        loggingFile.LogError(ex, "error.log", file); 
        Console.WriteLine($"An error occurred while processing the file '{file}'. Details have been logged to 'error.log'.");
        }
        catch (Exception logEx){

            Console.WriteLine($"Failed to log error for file '{file}'. Exception: {logEx.Message}"); 
        }

        failedIngestions++;
        
    }
}

Console.WriteLine($"Total Successful Ingestions: {successfulIngestions}, Total Failed Ingestions: {failedIngestions}, Total Records Processed: {totalRecordsProcessed}"); 