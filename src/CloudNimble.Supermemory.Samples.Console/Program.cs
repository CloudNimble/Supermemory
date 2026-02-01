using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("CloudNimble.Supermemory Sample Application");
Console.WriteLine("==========================================");
Console.WriteLine();

// Build the host with configuration from:
// - appsettings.json
// - appsettings.{Environment}.json
// - Environment variables (Supermemory__ApiKey)
// - User secrets (in Development)
var builder = Host.CreateApplicationBuilder(args);

// Add Supermemory services using the AOT-compatible overload
// Configuration is read manually and passed to the options
builder.Services.AddSupermemory(options =>
{
    var section = builder.Configuration.GetSection("Supermemory");
    options.ApiKey = section["ApiKey"];

    var baseUrl = section["BaseUrl"];
    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        options.BaseUrl = baseUrl;
    }

    var timeoutStr = section["Timeout"];
    if (!string.IsNullOrWhiteSpace(timeoutStr) && TimeSpan.TryParse(timeoutStr, out var timeout))
    {
        options.Timeout = timeout;
    }

    var maxRetriesStr = section["MaxRetries"];
    if (!string.IsNullOrWhiteSpace(maxRetriesStr) && int.TryParse(maxRetriesStr, out var maxRetries))
    {
        options.MaxRetries = maxRetries;
    }
});

using var host = builder.Build();

// Get the configured client from DI
var client = host.Services.GetRequiredService<SupermemoryClient>();

try
{
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
    Console.WriteLine("Please configure your API key using one of these methods:");
    Console.WriteLine("  1. User secrets: dotnet user-secrets set \"Supermemory:ApiKey\" \"your-api-key\"");
    Console.WriteLine("  2. Environment variable: Supermemory__ApiKey=your-api-key");
    Console.WriteLine("  3. appsettings.json: { \"Supermemory\": { \"ApiKey\": \"your-api-key\" } }");
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
