using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Represents a request to search memories (v4 API).
    /// </summary>
    public class SearchMemoriesRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the search query string.
        /// </summary>
        [JsonPropertyName("q")]
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the container tag to search within.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the search mode.
        /// </summary>
        [JsonPropertyName("searchMode")]
        public SearchMode? SearchMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of results to return.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets what to include in the response.
        /// </summary>
        [JsonPropertyName("include")]
        public SearchMemoriesInclude? Include { get; set; }

        /// <summary>
        /// Gets or sets whether to rerank results.
        /// </summary>
        [JsonPropertyName("rerank")]
        public bool? Rerank { get; set; }

        /// <summary>
        /// Gets or sets the filter expression.
        /// </summary>
        [JsonPropertyName("filters")]
        public FilterExpression? Filters { get; set; }

        #endregion

    }

    /// <summary>
    /// Specifies what to include in the search memories response.
    /// </summary>
    public class SearchMemoriesInclude
    {

        #region Properties

        /// <summary>
        /// Gets or sets whether to include chunks.
        /// </summary>
        [JsonPropertyName("chunks")]
        public bool? Chunks { get; set; }

        /// <summary>
        /// Gets or sets whether to include documents.
        /// </summary>
        [JsonPropertyName("documents")]
        public bool? Documents { get; set; }

        /// <summary>
        /// Gets or sets whether to include forgotten memories.
        /// </summary>
        [JsonPropertyName("forgottenMemories")]
        public bool? ForgottenMemories { get; set; }

        /// <summary>
        /// Gets or sets whether to include related memories.
        /// </summary>
        [JsonPropertyName("relatedMemories")]
        public bool? RelatedMemories { get; set; }

        /// <summary>
        /// Gets or sets whether to include summaries.
        /// </summary>
        [JsonPropertyName("summaries")]
        public bool? Summaries { get; set; }

        #endregion

    }

}
