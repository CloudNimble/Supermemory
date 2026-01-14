using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Represents the response from a memory search.
    /// </summary>
    public class SearchMemoriesResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        [JsonPropertyName("results")]
        public List<MemorySearchResult> Results { get; set; } = [];

        /// <summary>
        /// Gets or sets the time taken to execute the search in milliseconds.
        /// </summary>
        [JsonPropertyName("timing")]
        public double Timing { get; set; }

        /// <summary>
        /// Gets or sets the total number of results.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents a single memory search result.
    /// </summary>
    public class MemorySearchResult
    {

        #region Properties

        /// <summary>
        /// Gets or sets the result ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the similarity score.
        /// </summary>
        [JsonPropertyName("similarity")]
        public double Similarity { get; set; }

        /// <summary>
        /// Gets or sets when the result was last updated.
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the memory content (for memory results).
        /// </summary>
        [JsonPropertyName("memory")]
        public string? Memory { get; set; }

        /// <summary>
        /// Gets or sets the chunk content (for hybrid results).
        /// </summary>
        [JsonPropertyName("chunk")]
        public string? Chunk { get; set; }

        /// <summary>
        /// Gets or sets related chunks.
        /// </summary>
        [JsonPropertyName("chunks")]
        public List<object>? Chunks { get; set; }

        /// <summary>
        /// Gets or sets context information.
        /// </summary>
        [JsonPropertyName("context")]
        public object? Context { get; set; }

        /// <summary>
        /// Gets or sets related documents.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<object>? Documents { get; set; }

        /// <summary>
        /// Gets or sets the memory version.
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }

        #endregion

    }

}
