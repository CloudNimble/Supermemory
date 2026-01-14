using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents a request to configure a connection.
    /// </summary>
    public class ConfigureConnectionRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the resources to configure.
        /// </summary>
        [JsonPropertyName("resources")]
        public List<ConnectionResource> Resources { get; set; } = [];

        #endregion

    }

    /// <summary>
    /// Represents a resource in a connection configuration.
    /// </summary>
    public class ConnectionResource
    {

        #region Properties

        /// <summary>
        /// Gets or sets the resource ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the resource name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        #endregion

    }

}
