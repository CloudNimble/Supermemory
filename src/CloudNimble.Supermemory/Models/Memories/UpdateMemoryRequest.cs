using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Memories
{

    /// <summary>
    /// Represents a request to update an existing memory with new content.
    /// </summary>
    public class UpdateMemoryRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tag for the memory.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string ContainerTag { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new content for the memory.
        /// </summary>
        [JsonPropertyName("newContent")]
        public string NewContent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional ID of the memory to update.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the content to match for updating.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the metadata for the updated memory.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
