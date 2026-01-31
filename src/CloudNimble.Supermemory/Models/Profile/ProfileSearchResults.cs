using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Profile
{

    /// <summary>
    /// Represents search results within a profile response.
    /// </summary>
    public class ProfileSearchResults
    {

        #region Properties

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        [JsonPropertyName("results")]
        public List<object> Results { get; set; } = [];

        /// <summary>
        /// Gets or sets the time taken in milliseconds.
        /// </summary>
        [JsonPropertyName("timing")]
        public double Timing { get; set; }

        /// <summary>
        /// Gets or sets the total result count.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        #endregion

    }

}
