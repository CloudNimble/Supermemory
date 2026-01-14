using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents the response from configuring a connection.
    /// </summary>
    public class ConfigureConnectionResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets whether webhooks were registered.
        /// </summary>
        [JsonPropertyName("webhooksRegistered")]
        public bool? WebhooksRegistered { get; set; }

        #endregion

    }

}
