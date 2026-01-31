namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryChatHistoryProvider"/>.
    /// </summary>
    public class SupermemoryChatHistoryProviderOptions
    {

        #region Properties

        /// <summary>
        /// Gets or sets the default container tag for conversation isolation.
        /// Supports template placeholders: {userId}, {sessionId}, {tenantId}, {threadId}.
        /// </summary>
        public string? DefaultContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of messages to retrieve. Default: 50.
        /// </summary>
        public int MaxMessages { get; set; } = 50;

        /// <summary>
        /// Gets or sets the optional chat reducer for managing history size.
        /// </summary>
        public IChatReducer? ChatReducer { get; set; }

        /// <summary>
        /// Gets or sets when to apply the chat reducer. Default: <see cref="ChatReducerTriggerEvent.BeforeMessagesRetrieval"/>.
        /// </summary>
        public ChatReducerTriggerEvent ReducerTrigger { get; set; } = ChatReducerTriggerEvent.BeforeMessagesRetrieval;

        /// <summary>
        /// Gets or sets the custom ID prefix for stored documents. Default: "chat-".
        /// </summary>
        public string DocumentIdPrefix { get; set; } = "chat-";

        /// <summary>
        /// Gets or sets metadata to include with stored messages.
        /// </summary>
        public Dictionary<string, object>? DefaultMetadata { get; set; }

        /// <summary>
        /// Gets or sets whether to store context provider messages. Default: false.
        /// </summary>
        public bool StoreContextProviderMessages { get; set; }

        #endregion

    }

}
