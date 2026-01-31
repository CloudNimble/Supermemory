using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Represents a single search result.
    /// </summary>
    public class SearchResult
    {

        #region Properties

        /// <summary>
        /// Gets or sets the document ID.
        /// </summary>
        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document title.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        [JsonPropertyName("type")]
        public DocumentType Type { get; set; }

        /// <summary>
        /// Gets or sets the relevance score.
        /// </summary>
        [JsonPropertyName("score")]
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the document content.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the document summary.
        /// </summary>
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

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
        /// Gets or sets the matching chunks.
        /// </summary>
        [JsonPropertyName("chunks")]
        public List<SearchChunk>? Chunks { get; set; }

        #endregion

    }

}
