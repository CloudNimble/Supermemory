using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Settings
{

    /// <summary>
    /// Represents a request to update organization settings.
    /// </summary>
    public class UpdateSettingsRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the chunk size for document processing.
        /// </summary>
        [JsonPropertyName("chunkSize")]
        public int? ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets items to exclude from processing.
        /// </summary>
        [JsonPropertyName("excludeItems")]
        public object? ExcludeItems { get; set; }

        /// <summary>
        /// Gets or sets the filter prompt for processing.
        /// </summary>
        [JsonPropertyName("filterPrompt")]
        public string? FilterPrompt { get; set; }

        /// <summary>
        /// Gets or sets whether LLM filtering is enabled.
        /// </summary>
        [JsonPropertyName("shouldLLMFilter")]
        public bool? ShouldLlmFilter { get; set; }

        /// <summary>
        /// Gets or sets the GitHub OAuth client ID.
        /// </summary>
        [JsonPropertyName("githubOAuthClientId")]
        public string? GitHubOAuthClientId { get; set; }

        /// <summary>
        /// Gets or sets the GitHub OAuth client secret.
        /// </summary>
        [JsonPropertyName("githubOAuthClientSecret")]
        public string? GitHubOAuthClientSecret { get; set; }

        /// <summary>
        /// Gets or sets whether a custom GitHub key is enabled.
        /// </summary>
        [JsonPropertyName("githubOAuthCustomKeyEnabled")]
        public bool? GitHubOAuthCustomKeyEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Google Drive OAuth client ID.
        /// </summary>
        [JsonPropertyName("googleDriveOAuthClientId")]
        public string? GoogleDriveOAuthClientId { get; set; }

        /// <summary>
        /// Gets or sets the Google Drive OAuth client secret.
        /// </summary>
        [JsonPropertyName("googleDriveOAuthClientSecret")]
        public string? GoogleDriveOAuthClientSecret { get; set; }

        /// <summary>
        /// Gets or sets whether a custom Google Drive key is enabled.
        /// </summary>
        [JsonPropertyName("googleDriveOAuthCustomKeyEnabled")]
        public bool? GoogleDriveOAuthCustomKeyEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Notion OAuth client ID.
        /// </summary>
        [JsonPropertyName("notionOAuthClientId")]
        public string? NotionOAuthClientId { get; set; }

        /// <summary>
        /// Gets or sets the Notion OAuth client secret.
        /// </summary>
        [JsonPropertyName("notionOAuthClientSecret")]
        public string? NotionOAuthClientSecret { get; set; }

        /// <summary>
        /// Gets or sets whether a custom Notion key is enabled.
        /// </summary>
        [JsonPropertyName("notionOAuthCustomKeyEnabled")]
        public bool? NotionOAuthCustomKeyEnabled { get; set; }

        /// <summary>
        /// Gets or sets the OneDrive OAuth client ID.
        /// </summary>
        [JsonPropertyName("onedriveOAuthClientId")]
        public string? OneDriveOAuthClientId { get; set; }

        /// <summary>
        /// Gets or sets the OneDrive OAuth client secret.
        /// </summary>
        [JsonPropertyName("onedriveOAuthClientSecret")]
        public string? OneDriveOAuthClientSecret { get; set; }

        /// <summary>
        /// Gets or sets whether a custom OneDrive key is enabled.
        /// </summary>
        [JsonPropertyName("onedriveOAuthCustomKeyEnabled")]
        public bool? OneDriveOAuthCustomKeyEnabled { get; set; }

        #endregion

    }

}
