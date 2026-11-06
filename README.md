
# Memory-Safe File Ingestion Application

## 1. Project Overview

The Memory-Safe File Ingestion Application is a C# console application developed to demonstrate safe file handling, progressive data reading, resource management and exception logging.

The application processes multiple text files independently, reads their contents line by line, counts the number of records processed and produces an ingestion result for each file.

It also implements a custom logging mechanism to capture exceptions encountered during file processing. This allows developers to investigate failures without unnecessarily exposing technical information to the end user.

The primary objective is to demonstrate how fundamental C# and .NET concepts can be applied when developing a reliable file-processing application.

## 2. Project Objectives

The application was developed with the following objectives:

- Implement file input/output operations using C#.
- Read files progressively rather than loading their entire contents into memory.
- Demonstrate appropriate resource management through `IDisposable` and `using`.
- Implement custom exception logging.
- Handle failures independently so that one unsuccessful file does not interrupt the processing of subsequent files.
- Separate application flow, file processing and logging responsibilities.
- Return structured ingestion results using C# records.

## 3. Application Architecture

The application follows a simple separation-of-concerns approach.

Each component has a specific responsibility.

| Component | Responsibility |
|---|---|
| `Program.cs` | Controls application execution, maintains the list of files, handles exceptions and displays results. |
| `FileIngestionService.cs` | Responsible for opening, reading and processing individual files. |
| `LoggingFile.cs` | Responsible for recording exception information in a log file. |
| `IngestionResultRecord.cs` | Represents the outcome of a file ingestion operation. |

### Application workflow

1. The application starts and initialises a collection of filenames.
2. An instance of `FileIngestionService` is created.
3. The application iterates through the collection using a `foreach` loop.
4. Each file is passed to the `IngestFile()` method.
5. The service attempts to open and read the file.
6. Each line is read progressively and counted as a record.
7. Once processing finishes, an `IngestionResultRecord` is returned.
8. The application displays the ingestion results.
9. If an exception occurs, the application catches it and calls the custom logger.
10. Processing continues with the next file.

## 4. File Processing and Memory Management

### FileStream

`FileStream` provides access to a file as a stream of bytes.

The application uses it to open files in read-only mode:

- `FileMode.Open` opens an existing file.
- `FileAccess.Read` restricts the operation to reading.

This provides the underlying stream through which file data can be accessed.

### StreamReader

`StreamReader` is used to read and decode the file's byte stream into readable text.

Rather than loading the entire file into memory, the application uses `ReadLine()` to retrieve one line at a time.

For example:

```csharp
string? line;

while ((line = reader.ReadLine()) != null)
{
    numberOfRecordsProcessed++;
}
```

The loop continues until the end of the file is reached.

`ReadLine()` returns `null` when there are no more lines to read.

This approach is beneficial when processing large files because the application does not need to retain the entire file's contents in memory.

For this demonstration, each line is treated as one record.

### Resource Disposal

The application uses `using` declarations when creating its file-handling objects.

```csharp
using FileStream fileStream =
    new FileStream(filePath, FileMode.Open, FileAccess.Read);

using StreamReader reader = new StreamReader(fileStream);
```

These declarations ensure that the resources are disposed of when execution leaves their scope, including when an exception occurs.

The `StreamReader` also disposes of its underlying stream by default.

This is important because file streams use operating-system resources, including file handles. Failing to dispose of these resources can result in resource leaks and files remaining locked.

It is important to distinguish between resource disposal and garbage collection:

- `Dispose()` releases resources managed by an object.
- The garbage collector reclaims managed memory when objects are no longer reachable.

## 5. Structured Ingestion Results

The application uses a C# record to represent the outcome of each ingestion operation.

```csharp
public record IngestionResultRecord
{
    public string? FileName { get; init; }
    public int NumberOfRecordsProcessed { get; init; }
    public bool IngestionsSucceeded { get; init; }
}
```

The record contains three properties:

| Property | Purpose |
|---|---|
| `FileName` | Identifies the file that was processed. |
| `NumberOfRecordsProcessed` | Stores the number of lines processed. |
| `IngestionsSucceeded` | Indicates whether the ingestion operation completed successfully. |

Records are useful for representing data because they provide value-based equality and support convenient data copying.

The `init` accessor allows properties to be assigned during object initialisation while preventing ordinary reassignment afterwards.

The ingestion service returns this record after successfully processing a file.

## 6. Exception Handling

File operations can fail for several reasons, including:

- A file does not exist.
- The application does not have sufficient access permissions.
- A file becomes unavailable during processing.
- An unexpected input/output error occurs.

The application uses `try-catch` blocks to handle these situations.

The `FileIngestionService` throws an exception when it cannot process a file.

The calling code in `Program.cs` catches the exception.

This separation allows the service to focus on file processing while the application controls how failures are handled.

### Independent File Processing

An important design decision is placing the `try-catch` block inside the `foreach` loop.

This ensures that each file is handled independently.

For example, if `Birds.txt` cannot be processed, the application records the failure and proceeds to `Trucks.txt`.

One unsuccessful operation therefore does not automatically terminate the entire ingestion process.

## 7. Custom Exception Logging

The application implements a custom logging class called `LoggingFile`.

Its responsibility is to record exception information in a log file.

The method signature is:

```csharp
public void LogError(
    Exception ex,
    string filePath,
    string errorLocation)
```

The method accepts:

- `ex`: The complete exception object.
- `filePath`: The destination path of the log file.
- `errorLocation`: Context identifying where the error occurred.

The logger uses `StreamWriter` to write exception information.

```csharp
using StreamWriter writer =
    new StreamWriter(filePath, append: true);
```

The `append: true` argument ensures that new entries are added to the existing log file instead of replacing previous entries.

The logger records the timestamp, exception message, error location and exception type.

Passing the complete exception object is useful because it provides access to additional diagnostic information, including the exception type, message, stack trace and inner exception.

This allows the logging implementation to be extended when more detailed diagnostics are required.

## 8. Separation of User Feedback and Logging

The application distinguishes between information intended for the user and technical information intended for developers.

The console provides feedback about whether a file was processed successfully.

The log file stores technical exception details for troubleshooting.

This separation is important because users generally need understandable feedback, whereas developers need sufficient diagnostic information to investigate failures.

In a production implementation, user-facing messages should avoid exposing sensitive system information or internal file paths.

## 9. Testing and Current Results

The application was tested using three text files:

- `Fruits.txt`
- `Birds.txt`
- `Trucks.txt`

The observed console output was:

```text
First steps in File Input/Output in C#

File: Fruits.txt, Records Processed: 0, Ingestion Succeeded: True
File: Birds.txt, Records Processed: 0, Ingestion Succeeded: True
File: Trucks.txt, Records Processed: 0, Ingestion Succeeded: True
```

### Interpretation

All three files were successfully opened and read.

Each returned zero processed records, indicating that no lines were encountered during reading.

A file containing no records can still be successfully ingested. An empty file is not automatically considered an ingestion failure.

### Additional tests to perform

| Test scenario | Expected behaviour |
|---|---|
| Existing file containing records | Read the lines and return the correct count. |
| Empty file | Return zero records and a successful result. |
| Missing file | Throw an appropriate exception and log the failure. |
| Multiple files with one missing | Log the failed file and continue processing the remaining files. |
| File access denied | Handle the access exception and record diagnostic information. |
| Log file cannot be written | Handle the logging failure separately. |

## 10. Current Limitations and Future Improvements

The current implementation establishes the fundamental file-ingestion workflow. Further improvements can include:

1. **Robust logging:** Include stack traces, exception types and additional diagnostic context.
2. **Logging failure handling:** Protect the application against exceptions occurring inside the logging mechanism.
3. **Input validation:** Validate filenames and file paths before processing.
4. **Record validation:** Validate individual lines before counting them as successfully processed records.
5. **Ingestion summaries:** Provide a final summary of successful files, failed files and total records processed.
6. **Configurable file locations:** Allow input and log directories to be configured instead of relying on hardcoded paths.
7. **Automated testing:** Introduce unit tests to verify successful ingestion, empty files and exception handling.

## 11. Technical Concepts Demonstrated

The application provides practical exposure to the following C# and .NET concepts:

- Classes and objects
- Reference types
- Records and object initialisation
- File input/output
- `FileStream` and `StreamReader`
- Progressive file reading
- `IDisposable` and `using`
- Exception handling with `try-catch`
- Custom exception logging
- Collections using `List<string>`
- Iteration using `foreach`
- String interpolation
- Separation of concerns
- Resource management and garbage collection

## 12. Conclusion

The Memory-Safe File Ingestion Application demonstrates how fundamental C# and .NET concepts can be combined to develop a structured file-processing solution.

The implementation prioritises progressive reading, responsible resource disposal, independent file processing and separation of responsibilities.

Although the application is currently a foundational demonstration, its architecture provides a basis for further development into a more robust ingestion system.

The project also reinforces an important software engineering principle: reliable applications should not only perform their intended operations but should also manage resources responsibly and handle failures predictably.
