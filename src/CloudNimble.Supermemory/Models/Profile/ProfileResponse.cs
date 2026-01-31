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

}
