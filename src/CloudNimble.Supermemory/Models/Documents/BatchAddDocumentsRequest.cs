using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a request to add multiple documents in a single batch.
    /// </summary>
    public class BatchAddDocumentsRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of documents to add.
        /// Minimum 1, maximum 600 documents per batch.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<AddDocumentRequest> Documents { get; set; } = [];

        /// <summary>
        /// Gets or sets an optional container tag to apply to all documents.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets optional metadata to apply to all documents.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
