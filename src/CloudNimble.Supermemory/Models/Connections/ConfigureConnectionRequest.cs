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

}
