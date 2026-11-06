public class LoggingFile
{
    // LoggingFile is responsible for logging messages to a file.
    // The method LogError accepts an exception and a log file path as parameters.
    // It will write the exception details to the specified log file.
    public void LogError(Exception ex, string filePath, string errorLocation)
    {
        using StreamWriter writer = new StreamWriter(filePath, append: true); // Open the log file for writing. - Construct a StreamWriter object to write to the log file.
        //append: true means that the log messages will be appended to the end of the file, rather than overwriting the existing content.
        //ensures new entries are added rather than replacing previous ones.
        writer.WriteLine($"{DateTime.Now}: {ex.Message} [{errorLocation}] {ex.GetType().Name}"); // Write the exception message to the log file with a timestamp.
    }
}