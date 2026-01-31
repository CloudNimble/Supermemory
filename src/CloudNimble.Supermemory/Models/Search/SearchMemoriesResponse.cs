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

}
