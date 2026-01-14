using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents the response from a bulk delete operation.
    /// </summary>
    public class DeleteBulkResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the number of documents deleted.
        /// </summary>
        [JsonPropertyName("deletedCount")]
        public int DeletedCount { get; set; }

        /// <summary>
        /// Gets or sets any errors that occurred during deletion.
        /// </summary>
        [JsonPropertyName("errors")]
        public List<DeleteBulkError>? Errors { get; set; }

        /// <summary>
        /// Gets or sets the container tags that were affected.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        #endregion

    }

}
