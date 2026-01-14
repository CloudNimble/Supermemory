using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a request to add a single document to Supermemory.
    /// </summary>
    public class AddDocumentRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the content to extract and process into a document.
        /// This can be a URL to a website, a PDF, an image, a video, or plain text.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional container tag for grouping documents.
        /// Maximum 100 characters, alphanumeric with hyphens and underscores.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets a custom identifier from your database.
        /// Maximum 100 characters, alphanumeric with hyphens and underscores.
        /// </summary>
        [JsonPropertyName("customId")]
        public string? CustomId { get; set; }

        /// <summary>
        /// Gets or sets optional metadata for the document.
        /// Supports string, number, boolean, or string array values.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
