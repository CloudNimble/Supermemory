using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Profile
{

    /// <summary>
    /// Represents a request to get a profile.
    /// </summary>
    public class ProfileRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tag to filter the profile.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string ContainerTag { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional search query.
        /// </summary>
        [JsonPropertyName("q")]
        public string? Query { get; set; }

        /// <summary>
        /// Gets or sets the score threshold for results.
        /// </summary>
        [JsonPropertyName("threshold")]
        public double? Threshold { get; set; }

        #endregion

    }

}
