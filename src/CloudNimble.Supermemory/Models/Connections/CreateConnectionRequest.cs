using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents a request to create a new connection.
    /// </summary>
    public class CreateConnectionRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tags for the connection.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        /// <summary>
        /// Gets or sets the document limit for the connection.
        /// </summary>
        [JsonPropertyName("documentLimit")]
        public int? DocumentLimit { get; set; }

        /// <summary>
        /// Gets or sets metadata for the connection.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL after authentication.
        /// </summary>
        [JsonPropertyName("redirectUrl")]
        public string? RedirectUrl { get; set; }

        #endregion

    }

}
