using System.Text.Json.Serialization;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Represents the serializable state of a <see cref="SupermemoryChatHistoryProvider"/> session.
    /// </summary>
    public class SupermemoryChatHistoryProviderState
    {

        #region Properties

        /// <summary>
        /// Gets or sets the unique session/thread identifier for this conversation.
        /// </summary>
        [JsonPropertyName("sessionDbKey")]
        public string SessionDbKey { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets the container tag for conversation isolation.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the number of messages stored in this session.
        /// </summary>
        [JsonPropertyName("messageCount")]
        public int MessageCount { get; set; }

        /// <summary>
        /// Gets or sets the list of document IDs for stored messages.
        /// </summary>
        [JsonPropertyName("documentIds")]
        public List<string> DocumentIds { get; set; } = [];

        /// <summary>
        /// Gets or sets when this session was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp of the last message.
        /// </summary>
        [JsonPropertyName("lastMessageAt")]
        public DateTimeOffset? LastMessageAt { get; set; }

        #endregion

    }

}
