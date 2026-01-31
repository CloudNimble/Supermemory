using System.Text.Json.Serialization;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Represents the serializable state of a <see cref="SupermemoryContextProvider"/> session.
    /// </summary>
    public class SupermemoryContextProviderState
    {

        #region Properties

        /// <summary>
        /// Gets or sets the unique session identifier.
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets the container tag for this session.
        /// </summary>
        [JsonPropertyName("containerTag")]
        public string? ContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the list of document IDs stored during this session.
        /// </summary>
        [JsonPropertyName("storedMemoryIds")]
        public List<string> StoredMemoryIds { get; set; } = [];

        /// <summary>
        /// Gets or sets the timestamp of the last profile fetch.
        /// </summary>
        [JsonPropertyName("lastProfileFetch")]
        public DateTimeOffset? LastProfileFetch { get; set; }

        /// <summary>
        /// Gets or sets the current turn number for tracking conversation documents.
        /// </summary>
        [JsonPropertyName("turnNumber")]
        public int TurnNumber { get; set; }

        /// <summary>
        /// Gets or sets custom data that can be stored with the session.
        /// </summary>
        [JsonPropertyName("customData")]
        public Dictionary<string, object>? CustomData { get; set; }

        #endregion

    }

}
