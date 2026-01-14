using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a request to update an existing document.
    /// </summary>
    public class UpdateDocumentRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the new content for the document.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the container tag for the document.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the custom identifier.
        /// </summary>
        [JsonPropertyName("customId")]
        public string? CustomId { get; set; }

        /// <summary>
        /// Gets or sets the metadata for the document.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
