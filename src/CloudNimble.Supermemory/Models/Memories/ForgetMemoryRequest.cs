using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Memories
{

    /// <summary>
    /// Represents a request to forget (delete) a memory.
    /// </summary>
    public class ForgetMemoryRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tag for the memory to forget.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string ContainerTag { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional ID of the memory to forget.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the content to match for forgetting.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the reason for forgetting the memory.
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        #endregion

    }

}
