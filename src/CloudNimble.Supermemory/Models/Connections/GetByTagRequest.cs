using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Connections
{

    /// <summary>
    /// Represents a request to get a connection by container tags.
    /// </summary>
    public class GetByTagRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tags to match.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string> ContainerTags { get; set; } = [];

        #endregion

    }

}
