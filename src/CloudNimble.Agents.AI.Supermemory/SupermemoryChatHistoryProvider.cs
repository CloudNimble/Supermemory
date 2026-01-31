using System.Text.Json;
using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Models.Documents;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Provides Supermemory-backed conversation history storage for Microsoft Agent Framework.
    /// Stores chat messages as documents using the Documents API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This provider enables persistent conversation history by:
    /// <list type="bullet">
    /// <item><description>Storing each message turn as a Supermemory document</description></item>
    /// <item><description>Retrieving conversation history for session resumption</description></item>
    /// <item><description>Supporting container tag isolation for multi-user/multi-tenant scenarios</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public sealed class SupermemoryChatHistoryProvider : ChatHistoryProvider
    {

        #region Private Members

        private readonly SupermemoryClient _client;
        private readonly SupermemoryChatHistoryProviderOptions _options;
        private readonly IContainerTagResolver _containerTagResolver;
        private SupermemoryChatHistoryProviderState _state;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current state for external access.
        /// </summary>
        public SupermemoryChatHistoryProviderState State => _state;

        /// <summary>
        /// Gets or sets the container tag for conversation isolation.
        /// </summary>
        public string? ContainerTag
        {
            get => _state.ContainerTag;
            set => _state.ContainerTag = value;
        }

        /// <summary>
        /// Gets the session/thread ID for this conversation.
        /// </summary>
        public string SessionDbKey => _state.SessionDbKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for a fresh conversation.
        /// </summary>
        /// <param name="client">The Supermemory client for API calls.</param>
        /// <param name="options">Optional configuration options.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
        public SupermemoryChatHistoryProvider(
            SupermemoryClient client,
            SupermemoryChatHistoryProviderOptions? options = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? new SupermemoryChatHistoryProviderOptions();
            _containerTagResolver = containerTagResolver ?? new DefaultContainerTagResolver();
            _state = new SupermemoryChatHistoryProviderState();

            // Apply default container tag if specified
            if (!string.IsNullOrEmpty(_options.DefaultContainerTag))
            {
                _state.ContainerTag = _containerTagResolver.Resolve(
                    _options.DefaultContainerTag,
                    new ContainerTagContext { SessionId = _state.SessionDbKey });
            }
        }

        /// <summary>
        /// Creates an instance from serialized state (conversation resumption).
        /// </summary>
        /// <param name="client">The Supermemory client for API calls.</param>
        /// <param name="serializedState">The previously serialized state.</param>
        /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
        /// <param name="options">Optional configuration options.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
        public SupermemoryChatHistoryProvider(
            SupermemoryClient client,
            JsonElement serializedState,
            JsonSerializerOptions? jsonSerializerOptions = null,
            SupermemoryChatHistoryProviderOptions? options = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? new SupermemoryChatHistoryProviderOptions();
            _containerTagResolver = containerTagResolver ?? new DefaultContainerTagResolver();

            if (serializedState.ValueKind is JsonValueKind.Object)
            {
                var jso = jsonSerializerOptions ?? SupermemoryAgentFrameworkJsonContext.Default.Options;
                var typeInfo = jso.GetTypeInfo(typeof(SupermemoryChatHistoryProviderState));
                _state = serializedState.Deserialize(typeInfo) as SupermemoryChatHistoryProviderState
                    ?? new SupermemoryChatHistoryProviderState();
            }
            else
            {
                _state = new SupermemoryChatHistoryProviderState();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves conversation messages from Supermemory.
        /// </summary>
        /// <param name="context">The invoking context containing request messages.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of chat messages in chronological order.</returns>
        public override async ValueTask<IEnumerable<ChatMessage>> InvokingAsync(
            InvokingContext context,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);

            var containerTag = _state.ContainerTag;
            if (string.IsNullOrEmpty(containerTag) || string.IsNullOrEmpty(_state.SessionDbKey))
            {
                return [];
            }

            try
            {
                // List documents for this session
                var response = await _client.Documents.ListAsync(new ListDocumentsRequest
                {
                    ContainerTags = [containerTag],
                    Limit = _options.MaxMessages * 2 // Account for request/response pairs
                }, cancellationToken).ConfigureAwait(false);

                var messages = new List<ChatMessageWrapper>();

                foreach (var doc in response.Documents)
                {
                    // Filter by session - check if customId matches our pattern
                    if (doc.CustomId?.StartsWith($"{_options.DocumentIdPrefix}{_state.SessionDbKey}-", StringComparison.Ordinal) != true)
                    {
                        continue;
                    }

                    // Try to deserialize the content as a ChatMessageWrapper
                    if (!string.IsNullOrEmpty(doc.Content))
                    {
                        try
                        {
                            var wrapper = JsonSerializer.Deserialize(
                                doc.Content,
                                SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

                            if (wrapper != null)
                            {
                                messages.Add(wrapper);
                            }
                        }
                        catch
                        {
                            // Skip malformed messages
                        }
                    }
                }

                // Sort by timestamp and limit
                var sortedMessages = messages
                    .OrderBy(m => m.Timestamp)
                    .Take(_options.MaxMessages)
                    .ToList();

                // Apply reducer if configured
                if (_options.ChatReducer != null && _options.ReducerTrigger == ChatReducerTriggerEvent.BeforeMessagesRetrieval)
                {
                    sortedMessages = (await _options.ChatReducer.ReduceAsync(sortedMessages, cancellationToken).ConfigureAwait(false)).ToList();
                }

                // Convert to ChatMessage
                return sortedMessages.Select(ConvertToChatMessage);
            }
            catch
            {
                // Graceful degradation - return empty on error
                return [];
            }
        }

        /// <summary>
        /// Stores new messages to Supermemory.
        /// </summary>
        /// <param name="context">The invoked context containing request and response messages.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public override async ValueTask InvokedAsync(
            InvokedContext context,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.InvokeException is not null)
            {
                return;
            }

            var containerTag = _state.ContainerTag;
            if (string.IsNullOrEmpty(containerTag))
            {
                return;
            }

            try
            {
                // Collect all messages to store
                var messagesToStore = new List<ChatMessage>();

                // Add request messages
                messagesToStore.AddRange(context.RequestMessages);

                // Add context provider messages if configured
                if (_options.StoreContextProviderMessages && context.AIContextProviderMessages != null)
                {
                    messagesToStore.AddRange(context.AIContextProviderMessages);
                }

                // Add response messages
                if (context.ResponseMessages != null)
                {
                    messagesToStore.AddRange(context.ResponseMessages);
                }

                var now = DateTimeOffset.UtcNow;

                foreach (var message in messagesToStore)
                {
                    if (string.IsNullOrWhiteSpace(message.Text))
                    {
                        continue;
                    }

                    var messageId = Guid.NewGuid().ToString("N");
                    var wrapper = new ChatMessageWrapper
                    {
                        Id = messageId,
                        Role = message.Role.Value,
                        Content = message.Text,
                        Timestamp = now,
                        Metadata = _options.DefaultMetadata
                    };

                    var customId = $"{_options.DocumentIdPrefix}{_state.SessionDbKey}-{messageId}";

                    var content = JsonSerializer.Serialize(wrapper, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

                    var response = await _client.Documents.AddAsync(new AddDocumentRequest
                    {
                        Content = content,
                        ContainerTag = containerTag,
                        CustomId = customId,
                        Metadata = new Dictionary<string, object>
                        {
                            ["sessionDbKey"] = _state.SessionDbKey,
                            ["messageId"] = messageId,
                            ["role"] = message.Role.Value,
                            ["timestamp"] = now.ToString("o")
                        }
                    }, cancellationToken).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(response.Id))
                    {
                        _state.DocumentIds.Add(response.Id);
                    }

                    _state.MessageCount++;
                }

                _state.LastMessageAt = now;

                // Apply reducer after adding if configured
                if (_options.ChatReducer != null && _options.ReducerTrigger == ChatReducerTriggerEvent.AfterMessageAdded)
                {
                    // Note: For Supermemory, we'd need to implement deletion for reduced messages
                    // This is left as a future enhancement
                }
            }
            catch
            {
                // Graceful degradation - don't fail the agent if storage fails
            }
        }

        /// <summary>
        /// Serializes state for session persistence.
        /// </summary>
        /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
        /// <returns>A <see cref="JsonElement"/> representation of the state.</returns>
        public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var jso = jsonSerializerOptions ?? SupermemoryAgentFrameworkJsonContext.Default.Options;
            var typeInfo = jso.GetTypeInfo(typeof(SupermemoryChatHistoryProviderState));
            return JsonSerializer.SerializeToElement(_state, typeInfo);
        }

        #endregion

        #region Public Methods - Service Provider

        /// <inheritdoc/>
        public override object? GetService(Type serviceType, object? serviceKey = null)
        {
            ArgumentNullException.ThrowIfNull(serviceType);

            if (serviceKey is null)
            {
                if (serviceType == typeof(SupermemoryClient))
                {
                    return _client;
                }

                if (serviceType == typeof(SupermemoryChatHistoryProviderOptions))
                {
                    return _options;
                }

                if (serviceType == typeof(SupermemoryChatHistoryProviderState))
                {
                    return _state;
                }
            }

            return base.GetService(serviceType, serviceKey);
        }

        #endregion

        #region Private Methods

        private static ChatMessage ConvertToChatMessage(ChatMessageWrapper wrapper)
        {
            var role = wrapper.Role.ToLowerInvariant() switch
            {
                "user" => ChatRole.User,
                "assistant" => ChatRole.Assistant,
                "system" => ChatRole.System,
                "tool" => ChatRole.Tool,
                _ => new ChatRole(wrapper.Role)
            };

            return new ChatMessage(role, wrapper.Content);
        }

        #endregion

    }

}
