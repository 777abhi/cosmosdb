# cosmosdb

## Overview
This repository contains a C# console application that demos basic data workflows around CSV files, SQL Server, and Azure Cosmos DB. It includes helper utilities for combining CSV files, extracting file metadata, creating SQL tables from CSV data, and a Cosmos DB sample that inserts and queries family documents before comparing results against a SQL query.

## Existing Features

### CSV utilities
- Combine multiple CSV files into a single output file, skipping duplicate headers (`CSVHelpers.CombineCsvFiles`).
- Combine CSV files with mismatched headers by building a superset of columns and filling missing values (`CSVHelpers.CombineMisMatchedCsvFiles`).
- Convert in-memory dictionaries to `DataTable` and write them to CSV (`DataTableHelper.ToDataTable`, `DataTableHelper.ToCSV`).
- Read file metadata (attributes, timestamps, size, paths) for files in a directory filtered by extension (`CSVHelpers.GetFileProperties`).
- Basic temporary file creation/deletion example (`CSVHelpers.DeleteFiles`).

### SQL Server utilities
- Query a table and print rows to the console (`SQLHelpers.ReadOrderData`).
- Run a SQL query against a configured database and compare results to an expected Cosmos DB value (`SQLHelpers.ConnectToSQLANDQuery`).
- Create SQL tables from CSV files and load data into those tables (`CreateSQLTable.CreateSQLTablesFromCSV`).
- Load multiple CSV files with shared column names into a target table and archive the source files (`CreateSQLTable.CreateSQLTablesFromMultipleCSVWithDifferentDataAndSameColumnsName`).

### Cosmos DB demo
- Connect to Cosmos DB with the DocumentClient SDK and insert sample family documents (`CosmosDBConnect.GetStartedDemo`).
- Query Cosmos DB using SQL syntax and print results (`CosmosDBConnect.ExecuteSimpleQuery`).
- Optional helpers to replace or delete Cosmos DB documents (`CosmosDBConnect.ReplaceFamilyDocument`, `CosmosDBConnect.DeleteFamilyDocument`).

### Entry point behavior
- `Program.Main` is wired to call `CSVHelpers.GetFileProperties` by default, with other sample calls commented out for manual testing.

## Potential Next Incremental Features

### Configuration and setup
- Move hard-coded paths/connection strings into configuration or environment variables and document them in the README.
- Add a simple CLI or argument parsing to choose which workflow (CSV merge, SQL load, Cosmos demo) to run.
- Add a sample `.env` or `appsettings.json` template to make configuration clearer and safer.

### CSV processing improvements
- Add streaming CSV reading/writing to handle large files without loading them into memory.
- Add configurable header normalization (trim, case folding) and delimiter detection.
- Add validation and error reporting for malformed rows or mismatched column counts.

### SQL and Cosmos enhancements
- Use parameterized queries for SQL inserts and selects to avoid SQL injection and handle special characters safely.
- Add retry logic and transient-fault handling for Cosmos DB and SQL connections.
- Add support for bulk insert or batching to improve performance for large datasets.

### Testing and quality
- Add unit tests for CSV combination logic and data-table conversions.
- Add integration tests for SQL and Cosmos DB flows with test containers or local emulators.
- Introduce structured logging and optional verbose output flags.

### Documentation and samples
- Add a quickstart section showing how to run each workflow with sample data.
- Provide example CSV files and a sample database schema for local testing.

## Project Structure
- `Program.cs`: Console app entry point with sample invocation hooks.
- `helpers/CSVHelpers.cs`: CSV combination utilities, file metadata extraction, and `DataTable` helpers.
- `helpers/CreateSQLTable.cs`: SQL table creation and CSV loading workflows.
- `helpers/SQLHelpers.cs`: SQL query helpers and comparison logic.
- `helpers/CosmosDBConnect.cs`: Cosmos DB demo and document model definitions.
- `App.config`: Application settings and connection strings.
- `testconsoleappcosmosdb.csproj`: Project file.

## Notes
- Update Cosmos DB endpoint/key and SQL connection strings before running the demos.
- Some paths in the sample code are hard-coded for demonstration and may need to be adjusted for your environment.
