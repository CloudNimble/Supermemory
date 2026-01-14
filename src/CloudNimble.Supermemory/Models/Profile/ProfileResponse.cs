using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Profile
{

    /// <summary>
    /// Represents a profile response.
    /// </summary>
    public class ProfileResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the profile information.
        /// </summary>
        [JsonPropertyName("profile")]
        public ProfileInfo Profile { get; set; } = new();

        /// <summary>
        /// Gets or sets the optional search results.
        /// </summary>
        [JsonPropertyName("searchResults")]
        public ProfileSearchResults? SearchResults { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents profile information.
    /// </summary>
    public class ProfileInfo
    {

        #region Properties

        /// <summary>
        /// Gets or sets recent memories (dynamic profile data).
        /// </summary>
        [JsonPropertyName("dynamic")]
        public List<string> Dynamic { get; set; } = [];

        /// <summary>
        /// Gets or sets long-term relevant information (static profile data).
        /// </summary>
        [JsonPropertyName("static")]
        public List<string> Static { get; set; } = [];

        #endregion

    }

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
