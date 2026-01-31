using Azure.AI.OpenAI;
using Azure.Identity;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

Console.WriteLine("Supermemory + Microsoft Agent Framework Demo");
Console.WriteLine("=============================================\n");

// Get configuration from environment variables
var supermemoryApiKey = Environment.GetEnvironmentVariable("SUPERMEMORY_API_KEY");
var azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");

if (string.IsNullOrEmpty(supermemoryApiKey))
{
    Console.WriteLine("Error: SUPERMEMORY_API_KEY environment variable is not set.");
    Console.WriteLine("Please set it and try again.");
    return;
}

if (string.IsNullOrEmpty(azureOpenAIEndpoint))
{
    Console.WriteLine("Error: AZURE_OPENAI_ENDPOINT environment variable is not set.");
    Console.WriteLine("Please set it and try again.");
    return;
}

// Create Supermemory client
using var supermemoryClient = new SupermemoryClient(supermemoryApiKey);
Console.WriteLine("Supermemory client initialized.");

// Create Azure OpenAI chat client
var azureClient = new AzureOpenAIClient(
    new Uri(azureOpenAIEndpoint),
    new DefaultAzureCredential());

var chatClient = azureClient.GetChatClient("gpt-4o-mini").AsIChatClient();
Console.WriteLine("Azure OpenAI client initialized.\n");

// Create agent with both Supermemory providers configured
var agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "MemoryAgent",
    Description = "An AI agent with persistent memory powered by Supermemory",
    ChatOptions = new ChatOptions
    {
        // Instructions will be augmented by the context provider
    }
}
.WithSupermemory(
    supermemoryClient,
    contextOptions: new SupermemoryContextProviderOptions
    {
        DefaultContainerTag = "demo-user-{sessionId}",
        RetrievalStrategy = MemoryRetrievalStrategy.ProfileFirst,
        EnableConversationStorage = true,
        StorageFormat = ConversationStorageFormat.Markdown,
        SearchLimit = 5,
        MinimumSimilarityScore = 0.6
    },
    historyOptions: new SupermemoryChatHistoryProviderOptions
    {
        DefaultContainerTag = "demo-user-{sessionId}",
        MaxMessages = 50,
        DocumentIdPrefix = "chat-msg-"
    }));

Console.WriteLine("Agent created with Supermemory integration.\n");

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
if (contextProvider != null)
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
if (contextProvider != null)
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
