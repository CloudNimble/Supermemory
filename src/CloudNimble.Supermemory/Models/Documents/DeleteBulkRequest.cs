using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a request to bulk delete documents.
    /// </summary>
    public class DeleteBulkRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of document IDs to delete.
        /// </summary>
        [JsonPropertyName("ids")]
        public List<string>? Ids { get; set; }

        /// <summary>
        /// Gets or sets the container tags to delete documents from.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        #endregion

    }

}
