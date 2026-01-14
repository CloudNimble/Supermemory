using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents a request to list connections.
    /// </summary>
    public class ListConnectionsRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tags to filter by.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        #endregion

    }

}
