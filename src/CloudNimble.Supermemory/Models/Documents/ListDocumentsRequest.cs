using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a request to list documents with optional filtering and pagination.
    /// </summary>
    public class ListDocumentsRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tags to filter by.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        /// <summary>
        /// Gets or sets the filter expression for querying documents.
        /// </summary>
        [JsonPropertyName("filters")]
        public FilterExpression? Filters { get; set; }

        /// <summary>
        /// Gets or sets whether to include the full document content in the response.
        /// </summary>
        [JsonPropertyName("includeContent")]
        public bool? IncludeContent { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of results to return.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        [JsonPropertyName("page")]
        public int? Page { get; set; }

        /// <summary>
        /// Gets or sets the field to sort by.
        /// </summary>
        [JsonPropertyName("sort")]
        public string? Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order (asc or desc).
        /// </summary>
        [JsonPropertyName("order")]
        public string? Order { get; set; }

        #endregion

    }

}
