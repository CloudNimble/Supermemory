using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Represents a request to search documents.
    /// </summary>
    public class SearchDocumentsRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the search query string.
        /// </summary>
        [JsonPropertyName("q")]
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum number of results to return.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets whether to rerank results.
        /// </summary>
        [JsonPropertyName("rerank")]
        public bool? Rerank { get; set; }

        /// <summary>
        /// Gets or sets whether to rewrite the query for better results.
        /// </summary>
        [JsonPropertyName("rewriteQuery")]
        public bool? RewriteQuery { get; set; }

        /// <summary>
        /// Gets or sets the filter expression.
        /// </summary>
        [JsonPropertyName("filters")]
        public FilterExpression? Filters { get; set; }

        /// <summary>
        /// Gets or sets the container tags to filter by.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        #endregion

    }

}
