using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Represents a matching chunk within a search result.
    /// </summary>
    public class SearchChunk
    {

        #region Properties

        /// <summary>
        /// Gets or sets the chunk content.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the chunk is relevant.
        /// </summary>
        [JsonPropertyName("isRelevant")]
        public bool IsRelevant { get; set; }

        /// <summary>
        /// Gets or sets the relevance score.
        /// </summary>
        [JsonPropertyName("score")]
        public double Score { get; set; }

        #endregion

    }

}
