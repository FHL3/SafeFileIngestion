
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


## Technical Concepts Demonstrated

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
- Resource management and garbage collections
