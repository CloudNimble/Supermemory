using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Profile
{

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

}
