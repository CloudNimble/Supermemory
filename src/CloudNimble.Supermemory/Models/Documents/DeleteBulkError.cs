using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents an error that occurred during bulk deletion.
    /// </summary>
    public class DeleteBulkError
    {

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the document that failed to delete.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        #endregion

    }

}
