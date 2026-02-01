# Supermemory .NET SDK

A rock-solid, AOT-compatible .NET SDK for [Supermemory](https://supermemory.ai) - the AI memory layer for your applications.

[![NuGet](https://img.shields.io/nuget/v/Supermemory.svg)](https://www.nuget.org/packages/Supermemory)
[![NuGet](https://img.shields.io/nuget/v/Agents.AI.Supermemory.svg?label=nuget%20%28agents%29)](https://www.nuget.org/packages/Agents.AI.Supermemory)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- **Full API Coverage** - Complete access to Supermemory's Documents, Search, Memories, Connections, Settings, and Profile APIs
- **AOT Compatible** - Fully supports ahead-of-time compilation and trimming with zero reflection in configuration binding
- **Multi-targeting** - Supports .NET 8, .NET 9, and .NET 10
- **Modern Configuration** - Uses `IOptions<T>` pattern with AOT-compatible `LoadFrom()` methods for configuration from any .NET source
- **IHttpClientFactory** - Proper HttpClient lifecycle management via typed client pattern
- **Dependency Injection** - First-class support for Microsoft.Extensions.DependencyInjection with `AddSupermemory()` and `AddSupermemoryAgent()`
- **Microsoft Agent Framework Integration** - Seamless integration with Microsoft Agent Framework for AI agents with persistent memory
- **Strongly Typed** - Full IntelliSense support with comprehensive XML documentation
- **Exception Hierarchy** - Granular exception types for precise error handling

## Packages

| Package | Description |
|---------|-------------|
| [Supermemory](https://www.nuget.org/packages/Supermemory) | Core API client for Supermemory |
| [Agents.AI.Supermemory](https://www.nuget.org/packages/Agents.AI.Supermemory) | Microsoft Agent Framework integration |

## Installation

### Core SDK

```bash
dotnet add package Supermemory
```

### Microsoft Agent Framework Integration

```bash
dotnet add package Agents.AI.Supermemory
```

## Quick Start

### Configuration

The SDK uses the standard .NET configuration pattern. Configuration can come from any source:
- `appsettings.json` / `appsettings.{Environment}.json`
- Environment variables (e.g., `Supermemory__ApiKey`)
- User secrets (for local development)
- Azure Key Vault, AWS Secrets Manager, etc.
- Command line arguments

**appsettings.json:**
```json
{
  "Supermemory": {
    "ApiKey": "your-api-key",
    "BaseUrl": "https://api.supermemory.ai",
    "Timeout": "00:01:00",
    "MaxRetries": 2
  }
}
```

**Or via user secrets (recommended for development):**
```bash
dotnet user-secrets set "Supermemory:ApiKey" "your-api-key"
```

**Or via environment variables:**
```bash
# Linux/macOS
export Supermemory__ApiKey=your-api-key

# Windows
set Supermemory__ApiKey=your-api-key
```

### Dependency Injection

```csharp
using CloudNimble.Supermemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Add Supermemory - configuration is automatically bound from the "Supermemory" section
builder.Services.AddSupermemory();

// Or with programmatic configuration overlay
builder.Services.AddSupermemory(options =>
{
    options.Timeout = TimeSpan.FromSeconds(120);
    options.MaxRetries = 5;
});

using var host = builder.Build();

// Inject where needed
var client = host.Services.GetRequiredService<SupermemoryClient>();
```

### Using the Client

```csharp
using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Search;

// Inject via constructor
public class MyService(SupermemoryClient supermemory)
{
    public async Task AddAndSearchAsync()
    {
        // Add a document
        var addResponse = await supermemory.Documents.AddAsync(new AddDocumentRequest
        {
            Content = "Supermemory is the AI memory layer for your applications.",
            ContainerTag = "my-app",
            Metadata = new Dictionary<string, object>
            {
                ["source"] = "documentation",
                ["category"] = "overview"
            }
        });

        Console.WriteLine($"Document added: {addResponse.Id}");

        // Search for relevant content
        var searchResponse = await supermemory.Search.SearchDocumentsAsync(new SearchDocumentsRequest
        {
            Query = "AI memory layer",
            Limit = 5,
            Rerank = true
        });

        foreach (var result in searchResponse.Results)
        {
            Console.WriteLine($"Found: {result.DocumentId} (score: {result.Score:F3})");
        }
    }
}
```

## API Resources

The `SupermemoryClient` provides access to all Supermemory API resources:

| Resource | Description |
|----------|-------------|
| `client.Documents` | Add, update, delete, and list documents |
| `client.Search` | Search documents and memories with semantic search |
| `client.Memories` | Update and forget memories |
| `client.Connections` | Manage external connections (Notion, Google Drive, etc.) |
| `client.Settings` | Configure organization settings |
| `client.Profile` | Get user profiles with static and dynamic memories |

### Documents

```csharp
// Add a document
var response = await client.Documents.AddAsync(new AddDocumentRequest
{
    Content = "Your content here",
    ContainerTag = "my-container"
});

// Add documents in batch
var batchResponse = await client.Documents.BatchAddAsync(new BatchAddDocumentsRequest
{
    ContainerTag = "my-container",
    Documents = [
        new() { Content = "Document 1" },
        new() { Content = "Document 2" }
    ]
});

// Get a document
var document = await client.Documents.GetAsync("document-id");

// List documents
var list = await client.Documents.ListAsync(new ListDocumentsRequest
{
    ContainerTags = ["my-container"],
    Limit = 20
});

// Update a document
await client.Documents.UpdateAsync("document-id", new UpdateDocumentRequest
{
    Content = "Updated content"
});

// Delete a document
await client.Documents.DeleteAsync("document-id");

// Bulk delete
await client.Documents.DeleteBulkAsync(new DeleteBulkRequest
{
    ContainerTags = ["container-to-delete"]
});
```

### Search

```csharp
// Search documents (v3 API)
var results = await client.Search.SearchDocumentsAsync(new SearchDocumentsRequest
{
    Query = "your search query",
    Limit = 10,
    Rerank = true,
    ContainerTags = ["my-container"]
});

// Search memories (v4 API - low latency)
var memories = await client.Search.SearchMemoriesAsync(new SearchMemoriesRequest
{
    Query = "your search query",
    ContainerTag = "my-container",
    SearchMode = SearchMode.Memories
});
```

### Profile

```csharp
// Get user profile with static and dynamic memories
var profile = await client.Profile.GetAsync(new ProfileRequest
{
    ContainerTags = ["user-123"],
    Query = "current context" // Optional: for contextual retrieval
});

Console.WriteLine("Static memories (long-term facts):");
foreach (var memory in profile.Profile.Static)
{
    Console.WriteLine($"  - {memory}");
}

Console.WriteLine("Dynamic memories (recent context):");
foreach (var memory in profile.Profile.Dynamic)
{
    Console.WriteLine($"  - {memory}");
}
```

## Exception Handling

The SDK provides a granular exception hierarchy for precise error handling:

```csharp
try
{
    var response = await client.Documents.GetAsync("document-id");
}
catch (SupermemoryAuthenticationException ex)
{
    // 401 Unauthorized - Invalid or missing API key
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (SupermemoryNotFoundException ex)
{
    // 404 Not Found - Resource doesn't exist
    Console.WriteLine($"Document not found: {ex.Message}");
}
catch (SupermemoryValidationException ex)
{
    // 400 Bad Request - Invalid request parameters
    Console.WriteLine($"Validation error: {ex.Message}");
    Console.WriteLine($"Details: {ex.ApiError?.Details}");
}
catch (SupermemoryRateLimitException ex)
{
    // 429 Too Many Requests - Rate limit exceeded
    Console.WriteLine($"Rate limited: {ex.Message}");
}
catch (SupermemoryApiException ex)
{
    // Other API errors with status code
    Console.WriteLine($"API error ({ex.StatusCode}): {ex.Message}");
}
catch (SupermemoryException ex)
{
    // Base exception for all Supermemory errors
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Microsoft Agent Framework Integration

The `Agents.AI.Supermemory` package provides seamless integration with [Microsoft Agent Framework](https://github.com/microsoft/agents), enabling AI agents with persistent memory.

### Two Complementary Providers

| Provider | Purpose | Supermemory APIs Used |
|----------|---------|----------------------|
| `SupermemoryContextProvider` | Semantic memory injection | Profile API + Search API |
| `SupermemoryChatHistoryProvider` | Conversation persistence | Documents API |

### Quick Setup (Recommended: Full DI Integration)

```csharp
using CloudNimble.Supermemory;
using CloudNimble.Agents.AI.Supermemory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Add Supermemory client (binds from "Supermemory" config section)
builder.Services.AddSupermemory();

// Add your chat client (e.g., Azure OpenAI)
builder.Services.AddSingleton<IChatClient>(sp =>
    sp.GetRequiredService<AzureOpenAIClient>()
      .GetChatClient("gpt-4o-mini")
      .AsIChatClient());

// Add Supermemory-enabled agent - all configuration from appsettings.json
builder.Services.AddSupermemoryAgent(
    configureAgent: options => options.Name = "MemoryAgent",
    configureContext: options => options.EnableConversationStorage = true,
    configureHistory: options => options.MaxMessages = 100);

using var host = builder.Build();

// Get the fully-configured agent from DI
var agent = host.Services.GetRequiredService<ChatClientAgent>();

// Start a conversation - the agent has persistent memory across sessions!
var session = await agent.GetNewSessionAsync();
var response = await agent.RunAsync("My name is Alice and I love cats.", session);
```

**appsettings.json:**
```json
{
  "Supermemory": {
    "ApiKey": "your-api-key"
  },
  "SupermemoryContext": {
    "DefaultContainerTag": "user-{userId}",
    "RetrievalStrategy": "ProfileFirst",
    "SearchLimit": 5
  },
  "SupermemoryHistory": {
    "DefaultContainerTag": "user-{userId}",
    "MaxMessages": 50
  }
}
```

### Manual Setup (Advanced)

For scenarios where you need more control:

```csharp
// Get clients from DI
var supermemoryClient = host.Services.GetRequiredService<SupermemoryClient>();
var chatClient = /* your chat client */;

// Create agent with Supermemory integration manually
var agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "MemoryAgent",
    Description = "An AI agent with persistent memory"
}
.WithSupermemory(
    supermemoryClient,
    contextOptions: new SupermemoryContextProviderOptions
    {
        DefaultContainerTag = "user-{userId}",
        RetrievalStrategy = MemoryRetrievalStrategy.ProfileFirst,
        EnableConversationStorage = true
    },
    historyOptions: new SupermemoryChatHistoryProviderOptions
    {
        DefaultContainerTag = "user-{userId}",
        MaxMessages = 50
    }));
```

### Context Provider Configuration

```csharp
var contextOptions = new SupermemoryContextProviderOptions
{
    // Container tag with template placeholders
    DefaultContainerTag = "tenant-{tenantId}-user-{userId}",

    // Memory retrieval strategy
    RetrievalStrategy = MemoryRetrievalStrategy.ProfileFirst, // or SearchOnly, ProfileOnly, Both

    // Profile API settings
    UseProfileApi = true,
    ProfileThreshold = 0.7,

    // Search API settings
    UseSearchApi = true,
    SearchMode = SearchMode.Memories,
    SearchLimit = 5,
    MinimumSimilarityScore = 0.7,
    RerankResults = false,

    // Conversation storage (for automatic memory extraction)
    EnableConversationStorage = true,
    StorageFormat = ConversationStorageFormat.Markdown,
    StoreUserMessagesOnly = false,

    // Custom instruction template
    InstructionTemplate = """
        ## User Context

        ### Known Facts
        {staticMemories}

        ### Recent Context
        {dynamicMemories}

        ### Relevant Memories
        {searchMemories}
        """
};
```

### Chat History Provider Configuration

```csharp
var historyOptions = new SupermemoryChatHistoryProviderOptions
{
    // Container tag for conversation isolation
    DefaultContainerTag = "session-{sessionId}",

    // Message limits
    MaxMessages = 50,

    // Document storage settings
    DocumentIdPrefix = "chat-",
    StoreContextProviderMessages = false
};
```

### Dynamic Container Tags

Switch user context during a conversation:

```csharp
var session = await agent.GetNewSessionAsync();

// Get the context provider
var contextProvider = session.GetService<SupermemoryContextProvider>();

// Change container tag mid-conversation
contextProvider.ContainerTag = "user-456";

// Or use template resolution
var resolver = new DefaultContainerTagResolver();
contextProvider.State.ContainerTag = resolver.Resolve(
    "tenant-{tenantId}-user-{userId}",
    new ContainerTagContext { TenantId = "acme", UserId = "john" });
```

### Session Serialization

Serialize and resume sessions:

```csharp
// Serialize the session
var serializedSession = session.Serialize();

// Store serializedSession (e.g., in Redis, database)

// Later, resume the session
var resumedSession = await agent.DeserializeSessionAsync(serializedSession);

// Continue the conversation with full context preserved
var response = await agent.RunAsync("What's my name?", resumedSession);
```

## Configuration Options

### SupermemoryClientOptions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ApiKey` | `string?` | `null` | Your Supermemory API key (required) |
| `BaseUrl` | `string` | `"https://api.supermemory.ai"` | API base URL |
| `Timeout` | `TimeSpan` | `00:01:00` | HTTP request timeout |
| `MaxRetries` | `int` | `2` | Maximum retry attempts for failed requests |

### Configuration Section Names

Each options class exposes a `SectionName` constant for the default configuration section:

| Options Class | Section Name |
|--------------|--------------|
| `SupermemoryClientOptions.SectionName` | `"Supermemory"` |
| `SupermemoryContextProviderOptions.SectionName` | `"SupermemoryContext"` |
| `SupermemoryChatHistoryProviderOptions.SectionName` | `"SupermemoryHistory"` |

You can customize the section name:

```csharp
// Use a different configuration section with programmatic overrides
builder.Services.AddSupermemory("MyCustomSection", options =>
{
    options.Timeout = TimeSpan.FromSeconds(120);
});
```

### AOT-Compatible Configuration

All configuration uses AOT-compatible manual binding via `LoadFrom()` methods - no reflection required:

```csharp
// The SDK automatically calls LoadFrom() internally, but you can also use it directly:
var options = new SupermemoryClientOptions();
options.LoadFrom(configuration.GetSection(SupermemoryClientOptions.SectionName));
```

## Samples

The repository includes sample applications:

- **CloudNimble.Supermemory.Samples.Console** - Basic SDK usage examples
- **CloudNimble.Agents.AI.Supermemory.Samples** - Microsoft Agent Framework integration demo

Run the samples:

```bash
# Configure API key via user secrets
cd src/CloudNimble.Supermemory.Samples.Console
dotnet user-secrets set "Supermemory:ApiKey" "your-api-key"
dotnet run

# Run the agent framework sample (requires Azure OpenAI)
cd src/CloudNimble.Agents.AI.Supermemory.Samples
dotnet user-secrets set "Supermemory:ApiKey" "your-api-key"
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-resource.openai.azure.com"
dotnet run
```

## Requirements

- .NET 8.0, .NET 9.0, or .NET 10.0
- A Supermemory API key ([Get one here](https://supermemory.ai))

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Links

- [Supermemory Website](https://supermemory.ai)
- [Supermemory API Documentation](https://docs.supermemory.ai)
- [Microsoft Agent Framework](https://github.com/microsoft/agents)
- [NuGet Package - Supermemory](https://www.nuget.org/packages/Supermemory)
- [NuGet Package - Agents.AI.Supermemory](https://www.nuget.org/packages/Agents.AI.Supermemory)
