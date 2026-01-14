using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents the response from listing connection resources.
    /// </summary>
    public class ConnectionResourcesResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of resources.
        /// </summary>
        [JsonPropertyName("resources")]
        public List<ConnectionResource> Resources { get; set; } = [];

        /// <summary>
        /// Gets or sets the total count of resources.
        /// </summary>
        [JsonPropertyName("total_count")]
        public int? TotalCount { get; set; }

        #endregion

    }

}
