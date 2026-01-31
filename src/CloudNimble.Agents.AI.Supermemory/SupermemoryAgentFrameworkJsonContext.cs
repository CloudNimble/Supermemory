using System.Text.Json.Serialization;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Provides AOT-compatible JSON serialization context for Supermemory Agent Framework types.
    /// </summary>
    [JsonSourceGenerationOptions(
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        WriteIndented = false,
        UseStringEnumConverter = true,
        GenerationMode = JsonSourceGenerationMode.Default)]

    // State classes
    [JsonSerializable(typeof(SupermemoryContextProviderState))]
    [JsonSerializable(typeof(SupermemoryChatHistoryProviderState))]

    // Container tag context
    [JsonSerializable(typeof(ContainerTagContext))]

    // Chat message wrapper
    [JsonSerializable(typeof(ChatMessageWrapper))]
    [JsonSerializable(typeof(List<ChatMessageWrapper>))]

    // Common types
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(List<string>))]

    public partial class SupermemoryAgentFrameworkJsonContext : JsonSerializerContext
    {

    }

}
