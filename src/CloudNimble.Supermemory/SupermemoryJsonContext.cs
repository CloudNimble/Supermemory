using System.Text.Json;
using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Connections;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Memories;
using CloudNimble.Supermemory.Models.Profile;
using CloudNimble.Supermemory.Models.Search;
using CloudNimble.Supermemory.Models.Settings;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Provides AOT-compatible JSON serialization context for Supermemory API types.
    /// This context uses source generators to pre-compile serialization code,
    /// eliminating the need for runtime reflection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Usage example:
    /// <code>
    /// var request = new AddDocumentRequest { Content = "https://example.com" };
    /// var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.AddDocumentRequest);
    /// </code>
    /// </para>
    /// </remarks>
    [JsonSourceGenerationOptions(
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        WriteIndented = false,
        UseStringEnumConverter = true,
        GenerationMode = JsonSourceGenerationMode.Default)]

    // Common types
    [JsonSerializable(typeof(JsonElement))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(List<string>))]

    // Enums
    [JsonSerializable(typeof(DocumentStatus))]
    [JsonSerializable(typeof(DocumentType))]
    [JsonSerializable(typeof(ConnectionProvider))]
    [JsonSerializable(typeof(SearchMode))]

    // Common models
    [JsonSerializable(typeof(ApiError))]
    [JsonSerializable(typeof(PaginationInfo))]
    [JsonSerializable(typeof(StatusResponse))]
    [JsonSerializable(typeof(FilterExpression))]
    [JsonSerializable(typeof(FilterCondition))]
    [JsonSerializable(typeof(List<FilterCondition>))]

    // Document request models
    [JsonSerializable(typeof(AddDocumentRequest))]
    [JsonSerializable(typeof(BatchAddDocumentsRequest))]
    [JsonSerializable(typeof(UpdateDocumentRequest))]
    [JsonSerializable(typeof(ListDocumentsRequest))]
    [JsonSerializable(typeof(DeleteBulkRequest))]
    [JsonSerializable(typeof(UploadFileRequest))]
    [JsonSerializable(typeof(List<AddDocumentRequest>))]

    // Document response models
    [JsonSerializable(typeof(DocumentResponse))]
    [JsonSerializable(typeof(ListDocumentsResponse))]
    [JsonSerializable(typeof(BatchAddDocumentsResponse))]
    [JsonSerializable(typeof(DeleteBulkResponse))]
    [JsonSerializable(typeof(DeleteBulkError))]
    [JsonSerializable(typeof(ListProcessingResponse))]
    [JsonSerializable(typeof(List<DocumentResponse>))]
    [JsonSerializable(typeof(List<StatusResponse>))]
    [JsonSerializable(typeof(List<DeleteBulkError>))]

    // Memory models
    [JsonSerializable(typeof(ForgetMemoryRequest))]
    [JsonSerializable(typeof(ForgetMemoryResponse))]
    [JsonSerializable(typeof(UpdateMemoryRequest))]
    [JsonSerializable(typeof(UpdateMemoryResponse))]

    // Search request models
    [JsonSerializable(typeof(SearchDocumentsRequest))]
    [JsonSerializable(typeof(SearchMemoriesRequest))]
    [JsonSerializable(typeof(SearchMemoriesInclude))]

    // Search response models
    [JsonSerializable(typeof(SearchDocumentsResponse))]
    [JsonSerializable(typeof(SearchMemoriesResponse))]
    [JsonSerializable(typeof(SearchResult))]
    [JsonSerializable(typeof(SearchChunk))]
    [JsonSerializable(typeof(MemorySearchResult))]
    [JsonSerializable(typeof(List<SearchResult>))]
    [JsonSerializable(typeof(List<SearchChunk>))]
    [JsonSerializable(typeof(List<MemorySearchResult>))]

    // Connection request models
    [JsonSerializable(typeof(CreateConnectionRequest))]
    [JsonSerializable(typeof(ListConnectionsRequest))]
    [JsonSerializable(typeof(ConfigureConnectionRequest))]
    [JsonSerializable(typeof(GetByTagRequest))]
    [JsonSerializable(typeof(ConnectionResource))]
    [JsonSerializable(typeof(List<ConnectionResource>))]

    // Connection response models
    [JsonSerializable(typeof(CreateConnectionResponse))]
    [JsonSerializable(typeof(ConnectionResponse))]
    [JsonSerializable(typeof(ConfigureConnectionResponse))]
    [JsonSerializable(typeof(DeleteConnectionResponse))]
    [JsonSerializable(typeof(ConnectionResourcesResponse))]
    [JsonSerializable(typeof(List<ConnectionResponse>), TypeInfoPropertyName = "ListConnectionResponse")]
    [JsonSerializable(typeof(List<ConnectionResource>), TypeInfoPropertyName = "ListConnectionResource")]

    // Settings models
    [JsonSerializable(typeof(SettingsResponse))]
    [JsonSerializable(typeof(UpdateSettingsRequest))]
    [JsonSerializable(typeof(UpdateSettingsResponse))]

    // Profile models
    [JsonSerializable(typeof(ProfileRequest))]
    [JsonSerializable(typeof(ProfileResponse))]
    [JsonSerializable(typeof(ProfileInfo))]
    [JsonSerializable(typeof(ProfileSearchResults))]

    public partial class SupermemoryJsonContext : JsonSerializerContext
    {

    }

}
