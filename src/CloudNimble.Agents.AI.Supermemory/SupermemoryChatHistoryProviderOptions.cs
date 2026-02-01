using Microsoft.Extensions.Configuration;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryChatHistoryProvider"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is designed to be used with the .NET Options pattern for configuration binding.
    /// Configuration can come from any .NET configuration source.
    /// </para>
    /// <para>
    /// Example configuration in <c>appsettings.json</c>:
    /// <code>
    /// {
    ///   "SupermemoryHistory": {
    ///     "DefaultContainerTag": "user-{sessionId}",
    ///     "MaxMessages": 50,
    ///     "DocumentIdPrefix": "chat-",
    ///     "StoreContextProviderMessages": false
    ///   }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public class SupermemoryChatHistoryProviderOptions
    {

        #region Constants

        /// <summary>
        /// The default configuration section name for <see cref="SupermemoryChatHistoryProviderOptions"/>.
        /// </summary>
        public const string SectionName = "SupermemoryHistory";

        #endregion

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

        #region Public Methods

        /// <summary>
        /// Loads configuration values from the specified section into this options instance.
        /// This method is AOT-compatible and does not use reflection.
        /// </summary>
        /// <param name="section">The configuration section to load from.</param>
        public void LoadFrom(IConfigurationSection section)
        {
            ArgumentNullException.ThrowIfNull(section);

            var defaultContainerTag = section[nameof(DefaultContainerTag)];
            if (!string.IsNullOrWhiteSpace(defaultContainerTag))
            {
                DefaultContainerTag = defaultContainerTag;
            }

            var maxMessagesStr = section[nameof(MaxMessages)];
            if (!string.IsNullOrWhiteSpace(maxMessagesStr) && int.TryParse(maxMessagesStr, out var maxMessages))
            {
                MaxMessages = maxMessages;
            }

            var reducerTriggerStr = section[nameof(ReducerTrigger)];
            if (!string.IsNullOrWhiteSpace(reducerTriggerStr) && Enum.TryParse<ChatReducerTriggerEvent>(reducerTriggerStr, out var reducerTrigger))
            {
                ReducerTrigger = reducerTrigger;
            }

            var documentIdPrefix = section[nameof(DocumentIdPrefix)];
            if (!string.IsNullOrWhiteSpace(documentIdPrefix))
            {
                DocumentIdPrefix = documentIdPrefix;
            }

            var storeContextProviderMessagesStr = section[nameof(StoreContextProviderMessages)];
            if (!string.IsNullOrWhiteSpace(storeContextProviderMessagesStr) && bool.TryParse(storeContextProviderMessagesStr, out var storeContextProviderMessages))
            {
                StoreContextProviderMessages = storeContextProviderMessages;
            }
        }

        #endregion

    }

}
