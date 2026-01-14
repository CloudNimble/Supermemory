using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Search;

Console.WriteLine("CloudNimble.Supermemory Sample Application");
Console.WriteLine("==========================================");
Console.WriteLine();

// Get API key from environment variable
var apiKey = Environment.GetEnvironmentVariable("SUPERMEMORY_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Please set the SUPERMEMORY_API_KEY environment variable.");
    Console.WriteLine("Example: set SUPERMEMORY_API_KEY=your-api-key");
    return;
}

try
{
    // Create the client
    using var client = new SupermemoryClient(apiKey);

    // Example 1: Add a document
    Console.WriteLine("Adding a document...");
    var addResponse = await client.Documents.AddAsync(new AddDocumentRequest
    {
        Content = "The quick brown fox jumps over the lazy dog. This is a sample document for demonstrating the Supermemory API.",
        ContainerTag = "demo-sample",
        Metadata = new Dictionary<string, object>
        {
            ["source"] = "sample-app",
            ["timestamp"] = DateTime.UtcNow.ToString("O")
        }
    });
    Console.WriteLine($"Document added with ID: {addResponse.Id}, Status: {addResponse.Status}");
    Console.WriteLine();

    // Example 2: List documents
    Console.WriteLine("Listing documents...");
    var listResponse = await client.Documents.ListAsync(new ListDocumentsRequest
    {
        Limit = 5
    });
    Console.WriteLine($"Found {listResponse.Documents.Count} documents:");
    foreach (var doc in listResponse.Documents)
    {
        Console.WriteLine($"  - {doc.Id}: {doc.Status} ({doc.Type})");
    }
    Console.WriteLine();

    // Example 3: Search documents
    Console.WriteLine("Searching for 'quick brown fox'...");
    var searchResponse = await client.Search.SearchDocumentsAsync(new SearchDocumentsRequest
    {
        Query = "quick brown fox",
        Limit = 5
    });
    Console.WriteLine($"Found {searchResponse.Total} results (took {searchResponse.Timing}ms):");
    foreach (var result in searchResponse.Results)
    {
        Console.WriteLine($"  - {result.DocumentId}: score={result.Score:F3}");
        if (!string.IsNullOrEmpty(result.Content))
        {
            var preview = result.Content.Length > 100
                ? result.Content[..100] + "..."
                : result.Content;
            Console.WriteLine($"    {preview}");
        }
    }
    Console.WriteLine();

    // Example 4: Get a specific document
    if (!string.IsNullOrEmpty(addResponse.Id))
    {
        Console.WriteLine($"Getting document {addResponse.Id}...");
        try
        {
            var getResponse = await client.Documents.GetAsync(addResponse.Id);
            Console.WriteLine($"Document: {getResponse.Id}");
            Console.WriteLine($"  Status: {getResponse.Status}");
            Console.WriteLine($"  Type: {getResponse.Type}");
            Console.WriteLine($"  Created: {getResponse.CreatedAt}");
        }
        catch (SupermemoryNotFoundException)
        {
            Console.WriteLine("Document not found (may still be processing).");
        }
    }
    Console.WriteLine();

    // Example 5: Get settings
    Console.WriteLine("Getting organization settings...");
    var settings = await client.Settings.GetAsync();
    Console.WriteLine($"Chunk Size: {settings.ChunkSize}");
    Console.WriteLine($"LLM Filter Enabled: {settings.ShouldLlmFilter}");
    Console.WriteLine();

    Console.WriteLine("Sample completed successfully!");
}
catch (SupermemoryAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
    Console.WriteLine("Please check your API key.");
}
catch (SupermemoryApiException ex)
{
    Console.WriteLine($"API error ({ex.StatusCode}): {ex.Message}");
}
catch (SupermemoryException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
