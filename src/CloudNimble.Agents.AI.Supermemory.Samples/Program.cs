using Azure.AI.OpenAI;
using Azure.Identity;
using CloudNimble.Agents.AI.Supermemory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Supermemory + Microsoft Agent Framework Demo");
Console.WriteLine("=============================================\n");

// Build the host with configuration from:
// - appsettings.json
// - appsettings.{Environment}.json
// - Environment variables (Supermemory__ApiKey, AzureOpenAI__Endpoint)
// - User secrets (in Development)
var builder = Host.CreateApplicationBuilder(args);

// Validate configuration
var azureOpenAIEndpoint = builder.Configuration["AzureOpenAI:Endpoint"];
if (string.IsNullOrWhiteSpace(azureOpenAIEndpoint))
{
    Console.WriteLine("Error: AzureOpenAI:Endpoint is not configured.");
    Console.WriteLine("Configure it using one of these methods:");
    Console.WriteLine("  1. User secrets: dotnet user-secrets set \"AzureOpenAI:Endpoint\" \"https://your-resource.openai.azure.com\"");
    Console.WriteLine("  2. Environment variable: AzureOpenAI__Endpoint=https://your-resource.openai.azure.com");
    Console.WriteLine("  3. appsettings.json: { \"AzureOpenAI\": { \"Endpoint\": \"https://your-resource.openai.azure.com\" } }");
    return;
}

// Add Supermemory services - configuration is automatically bound from the "Supermemory" section
builder.Services.AddSupermemory();

// Add Azure OpenAI client
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var endpoint = config["AzureOpenAI:Endpoint"]!;
    return new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential());
});

// Register IChatClient from Azure OpenAI
builder.Services.AddSingleton<IChatClient>(sp =>
    sp.GetRequiredService<AzureOpenAIClient>()
      .GetChatClient("gpt-4o-mini")
      .AsIChatClient());

// Add the Supermemory-enabled agent - everything is resolved from DI
builder.Services.AddSupermemoryAgent(
    configureAgent: options =>
    {
        options.Name = "MemoryAgent";
        options.Description = "An AI agent with persistent memory powered by Supermemory";
    },
    configureContext: options =>
    {
        options.DefaultContainerTag = "demo-user-{sessionId}";
        options.RetrievalStrategy = MemoryRetrievalStrategy.ProfileFirst;
        options.EnableConversationStorage = true;
        options.StorageFormat = ConversationStorageFormat.Markdown;
        options.SearchLimit = 5;
        options.MinimumSimilarityScore = 0.6;
    },
    configureHistory: options =>
    {
        options.DefaultContainerTag = "demo-user-{sessionId}";
        options.MaxMessages = 50;
        options.DocumentIdPrefix = "chat-msg-";
    });

using var host = builder.Build();

// Get the fully-configured agent from DI
var agent = host.Services.GetRequiredService<ChatClientAgent>();
Console.WriteLine("Agent initialized from DI.\n");

// Get a new session
var session = await agent.GetNewSessionAsync();

// Access providers for inspection and manipulation
var contextProvider = session.GetService<SupermemoryContextProvider>();
var historyProvider = session.GetService<SupermemoryChatHistoryProvider>();

Console.WriteLine($"Session ID (Context): {contextProvider?.State.SessionId}");
Console.WriteLine($"Session DB Key (History): {historyProvider?.SessionDbKey}");
Console.WriteLine($"Container Tag: {contextProvider?.ContainerTag}\n");

Console.WriteLine("Starting conversation...\n");
Console.WriteLine("-------------------------------------------");

// First exchange - introduce yourself
Console.WriteLine("User: My name is Robert and I'm a software architect who loves C# and .NET.");
var response = await agent.RunAsync(
    "My name is Robert and I'm a software architect who loves C# and .NET.",
    session);
Console.WriteLine($"Agent: {response}\n");

// Second exchange - add more context
Console.WriteLine("User: I work at a company called CloudNimble and I'm interested in AI memory systems.");
response = await agent.RunAsync(
    "I work at a company called CloudNimble and I'm interested in AI memory systems.",
    session);
Console.WriteLine($"Agent: {response}\n");

// Third exchange - test memory recall
Console.WriteLine("User: What do you know about me so far?");
response = await agent.RunAsync(
    "What do you know about me so far?",
    session);
Console.WriteLine($"Agent: {response}\n");

Console.WriteLine("-------------------------------------------\n");

// Demonstrate changing container tag mid-conversation
Console.WriteLine("Switching to a different user context...\n");
if (contextProvider is not null)
{
    contextProvider.ContainerTag = "demo-user-different";
    Console.WriteLine($"New Container Tag: {contextProvider.ContainerTag}\n");
}

Console.WriteLine("User: What's my name?");
response = await agent.RunAsync(
    "What's my name?",
    session);
Console.WriteLine($"Agent: {response}\n");

// Switch back and test recall
Console.WriteLine("Switching back to original user context...\n");
if (contextProvider is not null)
{
    contextProvider.ContainerTag = $"demo-user-{contextProvider.State.SessionId}";
    Console.WriteLine($"Container Tag: {contextProvider.ContainerTag}\n");
}

Console.WriteLine("User: Now what's my name?");
response = await agent.RunAsync(
    "Now what's my name?",
    session);
Console.WriteLine($"Agent: {response}\n");

Console.WriteLine("-------------------------------------------\n");

// Show session state
Console.WriteLine("Session Statistics:");
Console.WriteLine($"  - Context Turn Number: {contextProvider?.State.TurnNumber}");
Console.WriteLine($"  - Stored Memory IDs: {contextProvider?.State.StoredMemoryIds.Count}");
Console.WriteLine($"  - History Message Count: {historyProvider?.State.MessageCount}");
Console.WriteLine($"  - History Document IDs: {historyProvider?.State.DocumentIds.Count}");

// Serialize session for potential resumption
var serializedSession = session.Serialize();
Console.WriteLine("\nSession serialized successfully.");
Console.WriteLine("This session state can be stored and used to resume the conversation later.\n");

// Demonstrate session resumption
Console.WriteLine("Demonstrating session resumption...\n");
var resumedSession = await agent.DeserializeSessionAsync(serializedSession);
var resumedContextProvider = resumedSession.GetService<SupermemoryContextProvider>();
Console.WriteLine($"Resumed Session ID: {resumedContextProvider?.State.SessionId}");
Console.WriteLine($"Resumed Container Tag: {resumedContextProvider?.ContainerTag}");
Console.WriteLine($"Resumed Turn Number: {resumedContextProvider?.State.TurnNumber}");

Console.WriteLine("\n=============================================");
Console.WriteLine("Demo completed successfully!");
Console.WriteLine("\nCheck your Supermemory dashboard to see the stored memories and documents.");
