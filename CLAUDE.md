# General

Please write a high quality, general purpose solution. Implement a solution that works correctly for all valid inputs, not just the test cases. Do not hard-code values or create solutions that only work for specific test inputs. Instead, implement the actual logic that solves the problem generally.

Focus on understanding the problem requirements and implementing the correct algorithm. Tests are there to verify correctness, not to define the solution. Provide a principled implementation that follows best practices and software design principles.

If the task is unreasonable or infeasible, or if any of the tests are incorrect, please tell me. The solution should be robust, maintainable, and extendable.

For maximum efficiency, whenever you need to perform multiple independent operations, invoke all relevant tools simultaneously rather than sequentially. After receiving tool results, carefully reflect on their quality and determine optimal next steps before proceeding. Use your thinking to plan and iterate based on this new information, and then take the best next action.

If you create any temporary new files, scripts, or helper files for iteration, clean up these files by removing them at the end of the task.

## Guidelines

* Make only high confidence suggestions when reviewing code changes.
* Always use the latest version C#. When global.json targets .NET 8, use C# 12. When it targets .NET 9.0, use C# 13. When it uses .NET 10.0, target C# 14.
* Never change global.json unless explicitly asked to.
* Always prefer `.IsNullOrWhiteSpace()` over `.IsNullOrEmpty()` for strings.
* Always prefer `ArgumentException.ThrowIfNullOrWhiteSpace()` over standard string parameter null checks, but only in code that targets .NET 8 or later.
* Always prefer `ArgumentNullException.ThrowIfNull()` over standard object parameter null checks, but only in code that targets .NET 8 or later.
* Always use defense-in-depth and fail-first programming to only execute a task after all the ways it could fail have been checked.
* Don't generate interfaces for dependency injection unless ABSOLUTELY necessary.

# Formatting

* Apply code-formatting style defined in `.editorconfig`.
* Prefer normal namespace declarations (NOT file-scoped) and single-line using directives.
* Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
* Ensure that the final return statement of a method is on its own line.
* Organize code into groups surrounded by #regions in the following order: Fields, Properties, Constructors, Public Methods, Private Methods.
  - Region instructions should be surrounded by blank lines.
* Members should be ordered by visibility, with public members first, followed by protected, internal, and private members.
* Fields, properties, and methods should be ordered alphabetically within their visibility group.
* Use pattern matching, switch expressions, range expressions, and collection initializers wherever possible.
* Use `nameof` instead of string literals when referring to member names.
* Ensure that extensive XML doc comments are created for any APIs.
  * When applicable, include <example> and <code> documentation in the comments.
  * Only <param> tags should be on the same line as content.
  * The <remarks> tag should be the last one before the member declaration.

## Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

# Testing

## Guidelines
* The format for test projects is "BaseNamespace.Tests.SubjectMatter". For example, if the Main project is "CloudNimble.Common.Amazon", then the Test project is "CloudNimble.Common.Tests.Amazon".
* We use Microsoft.TestPlatform for test execution (run from the /src folder), MSTest v3 for test code, Breakdance, and FluentAssertions for tests.
* Do not emit "Act", "Arrange" or "Assert" comments.
* Do not use any mocking in tests. Ever.
* Copy existing style in nearby files for test method names and capitalization.
* Always prefer `.NotBeNullOrWhiteSpace()` over `.NotBeNullOrEmpty()` for testing strings.
* This document defines how to create baselines: "D:\GitHub\Breakdance\specs\baseline-testing.md"
  * Use the `dotnet breakdance generate` command to create or update baselines.
  * If tests that need the baselines fail, and you regenerated the baselines, then the code to generate them is wrong and you need to fix it.

## MSTest 4 with New Testing Platform

This project uses MSTest 4 with the Microsoft Testing Platform enabled (`EnableMSTestRunner=true`, `TestingPlatformDotnetTestSupport=true`).

**Run tests using `dotnet run`** (required on .NET 10 SDK):

```bash
# Run all tests for the Agent Framework integration
dotnet run --project src/CloudNimble.Agents.AI.Tests.Supermemory -f net9.0

# Run tests for the base Supermemory client
dotnet run --project src/CloudNimble.Supermemory.Tests
```

## Run Tests with Code Coverage

Code coverage requires the `Microsoft.Testing.Extensions.CodeCoverage` package (already included).

```bash
# Run tests with code coverage (Cobertura format)
dotnet run --project src/CloudNimble.Agents.AI.Tests.Supermemory -f net9.0 -- --coverage --coverage-output-format cobertura

# Coverage report will be generated in:
# src/CloudNimble.Agents.AI.Tests.Supermemory/bin/Debug/net9.0/TestResults/*.cobertura.xml
```

## Generate Coverage Report

Install the report generator tool and create a summary:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator -reports:"src/CloudNimble.Agents.AI.Tests.Supermemory/bin/Debug/net9.0/TestResults/*.cobertura.xml" -targetdir:coverage-report -reporttypes:TextSummary

# View summary
cat coverage-report/Summary.txt
```

For HTML report:

```bash
reportgenerator -reports:"src/CloudNimble.Agents.AI.Tests.Supermemory/bin/Debug/net9.0/TestResults/*.cobertura.xml" -targetdir:coverage-report -reporttypes:Html
```

# Documentation

## Source System

* We're using Mintlify.com for documentation
  * The user instructions ate at https://mintlify.com/docs/llms-full.txt
  * The schema for docs.json files is here: https://leaves.mintlify.com/schema/docs.json

## Process

* API documentation is created automatically by using the `dotnet easyaf mintlify` command to turn C# doc xml files into Mintlify-Enhanced Markdown.
* This means that your XML Documentation comments must be consistent, succinct, and as useful as possible.

# Build Process

* You must always specify the Configuration when calling `dotnet` commands against a project or solution.

Build the entire solution:

```bash
dotnet build src/CloudNimble.Supermemory.slnx -c Debug
```

Build a specific project:

```bash
dotnet build src/CloudNimble.Agents.AI.Supermemory -c Debug
```

## Important Windows File System Notes

* NEVER redirect output to `nul` on Windows. The string "nul" is a reserved device name in Windows and will create an undeletable file.
  * Instead of `> nul`, use `> $null` in PowerShell or `> NUL` (uppercase) in cmd.
  * Better yet, avoid output redirection entirely when not necessary.
* When checking if files/directories exist, use proper error handling instead of redirecting to nul.
* Example of what NOT to do: `dir /s /b "pattern" 2>nul`
* Example of what TO do: `dir /s /b "pattern" 2>&1 | Out-Null` or simply let errors display.

# Project Details

* You can find more details about what we're building and how we're building it in the `/specs` folder.

## Project Structure

```
src/
  CloudNimble.Supermemory/                    # Core Supermemory API client
  CloudNimble.Supermemory.Tests/              # Tests for core client
  CloudNimble.Supermemory.Samples.Console/    # Console sample for core client

  CloudNimble.Agents.AI.Supermemory/          # Microsoft Agent Framework integration
  CloudNimble.Agents.AI.Tests.Supermemory/    # Tests for Agent Framework integration
  CloudNimble.Agents.AI.Supermemory.Samples/  # Sample using Agent Framework
```

## Target Frameworks

All projects target .NET 8.0, .NET 9.0, and .NET 10.0.

## Run Sample Application

The sample application requires environment variables:

```bash
# Set environment variables
set SUPERMEMORY_API_KEY=your-api-key
set AZURE_OPENAI_ENDPOINT=https://your-resource.openai.azure.com

# Run the sample
dotnet run --project src/CloudNimble.Agents.AI.Supermemory.Samples
```

## Notes

- Tests use parallel execution (MSTest 4 `Parallelize` attribute)
- Projects are AOT-compatible with `IsAotCompatible=true`
- JSON serialization uses source generators for AOT support

# Final Thoughts
You can do it! Don't hold back. Give it your all.
