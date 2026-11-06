public class LoggingFile
{
    public void LogError(Exception ex, string filePath, string errorLocation)
    {
        using StreamWriter writer = new StreamWriter(filePath, append: true);
        writer.WriteLine($"{DateTime.Now}: {ex.Message} [{errorLocation}] {ex.GetType().Name}"); // Write the exception message to the log file with a timestamp.
    }
}