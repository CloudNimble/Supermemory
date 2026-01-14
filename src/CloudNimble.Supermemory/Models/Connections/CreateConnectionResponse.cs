using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents the response from creating a connection.
    /// </summary>
    public class CreateConnectionResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the connection ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authentication link for the user.
        /// </summary>
        [JsonPropertyName("authLink")]
        public string AuthLink { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the auth link expires in seconds.
        /// </summary>
        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect to after authentication.
        /// </summary>
        [JsonPropertyName("redirectsTo")]
        public string? RedirectsTo { get; set; }

        #endregion

    }

}
