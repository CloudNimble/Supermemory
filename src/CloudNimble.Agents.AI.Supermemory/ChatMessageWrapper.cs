using System.Text.Json.Serialization;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Wrapper for serializing chat messages to and from Supermemory documents.
    /// </summary>
    public class ChatMessageWrapper
    {

        #region Properties

        /// <summary>
        /// Gets or sets the unique message identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role of the message sender (e.g., "user", "assistant", "system").
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the message was created.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets optional metadata associated with the message.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        #endregion

    }

}
