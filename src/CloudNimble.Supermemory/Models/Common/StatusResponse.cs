using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Common
{

    /// <summary>
    /// Represents a simple status response with an ID and status.
    /// </summary>
    public class StatusResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        #endregion

    }

}
