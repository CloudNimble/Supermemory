using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Memories
{

    /// <summary>
    /// Represents the response from an update memory operation.
    /// </summary>
    public class UpdateMemoryResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the new memory version.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the memory content.
        /// </summary>
        [JsonPropertyName("memory")]
        public string Memory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version number of the updated memory.
        /// </summary>
        [JsonPropertyName("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent memory.
        /// </summary>
        [JsonPropertyName("parentMemoryId")]
        public string ParentMemoryId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the root memory in the version chain.
        /// </summary>
        [JsonPropertyName("rootMemoryId")]
        public string RootMemoryId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when this memory version was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        #endregion

    }

}
