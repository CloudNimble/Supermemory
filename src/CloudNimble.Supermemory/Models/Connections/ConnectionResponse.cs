using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents a connection to an external provider.
    /// </summary>
    public class ConnectionResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the connection ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the connection was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        [JsonPropertyName("provider")]
        public ConnectionProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the document limit for the connection.
        /// </summary>
        [JsonPropertyName("documentLimit")]
        public int? DocumentLimit { get; set; }

        /// <summary>
        /// Gets or sets the email associated with the connection.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets when the connection expires.
        /// </summary>
        [JsonPropertyName("expiresAt")]
        public DateTimeOffset? ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the connection metadata.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
