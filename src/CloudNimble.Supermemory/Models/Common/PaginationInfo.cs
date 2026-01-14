using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Common
{

    /// <summary>
    /// Represents pagination metadata for list responses.
    /// </summary>
    public class PaginationInfo
    {

        #region Properties

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets whether there are more pages available.
        /// </summary>
        [JsonPropertyName("hasMore")]
        public bool HasMore { get; set; }

        #endregion

    }

}
