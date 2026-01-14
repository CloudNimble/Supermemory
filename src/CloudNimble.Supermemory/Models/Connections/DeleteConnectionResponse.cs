using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents the response from deleting a connection.
    /// </summary>
    public class DeleteConnectionResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the deleted connection ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the provider of the deleted connection.
        /// </summary>
        [JsonPropertyName("provider")]
        public ConnectionProvider Provider { get; set; }

        #endregion

    }

}
