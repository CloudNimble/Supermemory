using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents the full details of a document from Supermemory.
    /// </summary>
    public class DocumentResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the document.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the document.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the document.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the summary of the document content.
        /// </summary>
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        [JsonPropertyName("type")]
        public DocumentType Type { get; set; }

        /// <summary>
        /// Gets or sets the processing status of the document.
        /// </summary>
        [JsonPropertyName("status")]
        public DocumentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets when the document was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the document was last updated.
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the custom identifier from your database.
        /// </summary>
        [JsonPropertyName("customId")]
        public string? CustomId { get; set; }

        /// <summary>
        /// Gets or sets the connection ID if the document came from a connection.
        /// </summary>
        [JsonPropertyName("connectionId")]
        public string? ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the source of the document.
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// Gets or sets the metadata for the document.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the container tags for the document.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        /// <summary>
        /// Gets or sets the source URL of the document.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the raw document content.
        /// </summary>
        [JsonPropertyName("raw")]
        public object? Raw { get; set; }

        /// <summary>
        /// Gets or sets the Open Graph image URL.
        /// </summary>
        [JsonPropertyName("ogImage")]
        public string? OgImage { get; set; }

        /// <summary>
        /// Gets or sets the token count of the processed document.
        /// </summary>
        [JsonPropertyName("tokenCount")]
        public int TokenCount { get; set; }

        /// <summary>
        /// Gets or sets the embedding model used for the summary.
        /// </summary>
        [JsonPropertyName("summaryEmbeddingModel")]
        public string? SummaryEmbeddingModel { get; set; }

        #endregion

    }

}
