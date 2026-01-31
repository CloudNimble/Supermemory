using System.Text;
using System.Text.Json;
using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Search;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Provides Supermemory-backed semantic memory for Microsoft Agent Framework agents.
    /// Uses the Profile API for static/dynamic memories and Search API for semantic retrieval.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This provider enhances AI agent interactions by:
    /// <list type="bullet">
    /// <item><description>Retrieving user profiles with static (long-term) and dynamic (recent) memories</description></item>
    /// <item><description>Searching for semantically relevant memories based on the current conversation</description></item>
    /// <item><description>Storing conversations for automatic memory extraction by Supermemory</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public sealed class SupermemoryContextProvider : AIContextProvider
    {

        #region Private Members

        private readonly SupermemoryClient _client;
        private readonly SupermemoryContextProviderOptions _options;
        private readonly IContainerTagResolver _containerTagResolver;
        private SupermemoryContextProviderState _state;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current state for external access and manipulation.
        /// </summary>
        public SupermemoryContextProviderState State => _state;

        /// <summary>
        /// Gets or sets the container tag for this session. Can be changed during conversation.
        /// </summary>
        public string? ContainerTag
        {
            get => _state.ContainerTag;
            set => _state.ContainerTag = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for a fresh session.
        /// </summary>
        /// <param name="client">The Supermemory client for API calls.</param>
        /// <param name="options">Optional configuration options.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
        public SupermemoryContextProvider(
            SupermemoryClient client,
            SupermemoryContextProviderOptions? options = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? new SupermemoryContextProviderOptions();
            _containerTagResolver = containerTagResolver ?? new DefaultContainerTagResolver();
            _state = new SupermemoryContextProviderState();

            // Apply default container tag if specified
            if (!string.IsNullOrEmpty(_options.DefaultContainerTag))
            {
                _state.ContainerTag = _containerTagResolver.Resolve(
                    _options.DefaultContainerTag,
                    new ContainerTagContext { SessionId = _state.SessionId });
            }
        }

        /// <summary>
        /// Creates an instance from serialized state (session resumption).
        /// </summary>
        /// <param name="client">The Supermemory client for API calls.</param>
        /// <param name="serializedState">The previously serialized state.</param>
        /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
        /// <param name="options">Optional configuration options.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
        public SupermemoryContextProvider(
            SupermemoryClient client,
            JsonElement serializedState,
            JsonSerializerOptions? jsonSerializerOptions = null,
            SupermemoryContextProviderOptions? options = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? new SupermemoryContextProviderOptions();
            _containerTagResolver = containerTagResolver ?? new DefaultContainerTagResolver();

            if (serializedState.ValueKind is JsonValueKind.Object)
            {
                var jso = jsonSerializerOptions ?? SupermemoryAgentFrameworkJsonContext.Default.Options;
                var typeInfo = jso.GetTypeInfo(typeof(SupermemoryContextProviderState));
                _state = serializedState.Deserialize(typeInfo) as SupermemoryContextProviderState
                    ?? new SupermemoryContextProviderState();
            }
            else
            {
                _state = new SupermemoryContextProviderState();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called before agent invocation. Retrieves profile and/or searches memories.
        /// </summary>
        /// <param name="context">The invoking context containing request messages.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="AIContext"/> with memory-enhanced instructions.</returns>
        public override async ValueTask<AIContext> InvokingAsync(
            InvokingContext context,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);

            var containerTag = _state.ContainerTag;
            if (string.IsNullOrEmpty(containerTag))
            {
                return new AIContext();
            }

            // Extract query text from request messages
            var queryText = string.Join(
                Environment.NewLine,
                context.RequestMessages
                    .Where(m => !string.IsNullOrWhiteSpace(m.Text))
                    .Select(m => m.Text));

            if (string.IsNullOrWhiteSpace(queryText))
            {
                return new AIContext();
            }

            var staticMemories = new List<string>();
            var dynamicMemories = new List<string>();
            var searchMemories = new List<string>();

            try
            {
                switch (_options.RetrievalStrategy)
                {
                    case MemoryRetrievalStrategy.ProfileFirst:
                        await RetrieveProfileMemoriesAsync(containerTag, queryText, staticMemories, dynamicMemories, cancellationToken).ConfigureAwait(false);
                        if (_options.UseSearchApi && (staticMemories.Count == 0 && dynamicMemories.Count == 0))
                        {
                            await RetrieveSearchMemoriesAsync(containerTag, queryText, searchMemories, cancellationToken).ConfigureAwait(false);
                        }
                        break;

                    case MemoryRetrievalStrategy.ProfileOnly:
                        await RetrieveProfileMemoriesAsync(containerTag, queryText, staticMemories, dynamicMemories, cancellationToken).ConfigureAwait(false);
                        break;

                    case MemoryRetrievalStrategy.SearchOnly:
                        await RetrieveSearchMemoriesAsync(containerTag, queryText, searchMemories, cancellationToken).ConfigureAwait(false);
                        break;

                    case MemoryRetrievalStrategy.Both:
                        var profileTask = RetrieveProfileMemoriesAsync(containerTag, queryText, staticMemories, dynamicMemories, cancellationToken);
                        var searchTask = RetrieveSearchMemoriesAsync(containerTag, queryText, searchMemories, cancellationToken);
                        await Task.WhenAll(profileTask, searchTask).ConfigureAwait(false);
                        break;
                }

                _state.LastProfileFetch = DateTimeOffset.UtcNow;
            }
            catch
            {
                // Graceful degradation - return empty context on error
                return new AIContext();
            }

            // Format the instructions
            var instructions = FormatInstructions(staticMemories, dynamicMemories, searchMemories);

            if (string.IsNullOrWhiteSpace(instructions))
            {
                return new AIContext();
            }

            return new AIContext
            {
                Instructions = instructions
            };
        }

        /// <summary>
        /// Called after agent invocation. Stores the conversation for automatic memory extraction.
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

            if (!_options.EnableConversationStorage)
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
                // Collect messages to store
                var messages = _options.StoreUserMessagesOnly
                    ? context.RequestMessages.Where(m => m.Role == ChatRole.User)
                    : context.RequestMessages.Concat(context.ResponseMessages ?? []);

                var content = FormatConversationContent(messages);

                if (string.IsNullOrWhiteSpace(content))
                {
                    return;
                }

                _state.TurnNumber++;

                var customId = $"conv-{_state.SessionId}-{_state.TurnNumber}";

                var metadata = new Dictionary<string, object>
                {
                    ["sessionId"] = _state.SessionId,
                    ["turnNumber"] = _state.TurnNumber,
                    ["timestamp"] = DateTimeOffset.UtcNow.ToString("o"),
                    ["source"] = "agent-framework"
                };

                if (_options.DefaultMetadata != null)
                {
                    foreach (var kvp in _options.DefaultMetadata)
                    {
                        metadata[kvp.Key] = kvp.Value;
                    }
                }

                var response = await _client.Documents.AddAsync(new AddDocumentRequest
                {
                    Content = content,
                    ContainerTag = containerTag,
                    CustomId = customId,
                    Metadata = metadata
                }, cancellationToken).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response.Id))
                {
                    _state.StoredMemoryIds.Add(response.Id);
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
            var typeInfo = jso.GetTypeInfo(typeof(SupermemoryContextProviderState));
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

                if (serviceType == typeof(SupermemoryContextProviderOptions))
                {
                    return _options;
                }

                if (serviceType == typeof(SupermemoryContextProviderState))
                {
                    return _state;
                }
            }

            return base.GetService(serviceType, serviceKey);
        }

        #endregion

        #region Private Methods

        private async Task RetrieveProfileMemoriesAsync(
            string containerTag,
            string query,
            List<string> staticMemories,
            List<string> dynamicMemories,
            CancellationToken cancellationToken)
        {
            if (!_options.UseProfileApi)
            {
                return;
            }

            var profileResponse = await _client.Profile.GetAsync(containerTag, query, cancellationToken).ConfigureAwait(false);

            if (profileResponse.Profile.Static?.Count > 0)
            {
                staticMemories.AddRange(profileResponse.Profile.Static);
            }

            if (profileResponse.Profile.Dynamic?.Count > 0)
            {
                dynamicMemories.AddRange(profileResponse.Profile.Dynamic);
            }
        }

        private async Task RetrieveSearchMemoriesAsync(
            string containerTag,
            string query,
            List<string> searchMemories,
            CancellationToken cancellationToken)
        {
            if (!_options.UseSearchApi)
            {
                return;
            }

            var searchRequest = new SearchMemoriesRequest
            {
                Query = query,
                ContainerTag = containerTag,
                Limit = _options.SearchLimit,
                Rerank = _options.RerankResults
            };

            var searchResponse = await _client.Search.SearchMemoriesAsync(searchRequest, cancellationToken).ConfigureAwait(false);

            foreach (var result in searchResponse.Results)
            {
                if (result.Similarity >= _options.MinimumSimilarityScore && !string.IsNullOrEmpty(result.Memory))
                {
                    searchMemories.Add(result.Memory);
                }
            }
        }

        private string FormatInstructions(
            List<string> staticMemories,
            List<string> dynamicMemories,
            List<string> searchMemories)
        {
            var staticContent = staticMemories.Count > 0
                ? string.Join("\n- ", ["", .. staticMemories])
                : "None available";

            var dynamicContent = dynamicMemories.Count > 0
                ? string.Join("\n- ", ["", .. dynamicMemories])
                : "None available";

            var searchContent = searchMemories.Count > 0
                ? string.Join("\n- ", ["", .. searchMemories])
                : "None available";

            return _options.InstructionTemplate
                .Replace("{staticMemories}", staticContent)
                .Replace("{dynamicMemories}", dynamicContent)
                .Replace("{searchMemories}", searchContent);
        }

        private string FormatConversationContent(IEnumerable<ChatMessage> messages)
        {
            var sb = new StringBuilder();

            foreach (var message in messages)
            {
                var role = message.Role.Value;
                var content = message.Text;

                if (string.IsNullOrWhiteSpace(content))
                {
                    continue;
                }

                switch (_options.StorageFormat)
                {
                    case ConversationStorageFormat.Markdown:
                        sb.AppendLine($"**{char.ToUpperInvariant(role[0])}{role[1..]}**: {content}");
                        sb.AppendLine();
                        break;

                    case ConversationStorageFormat.PlainText:
                        sb.AppendLine($"{char.ToUpperInvariant(role[0])}{role[1..]}: {content}");
                        break;

                    case ConversationStorageFormat.Json:
                        // For JSON format, we'll build a simpler format
                        sb.AppendLine($"[{role}] {content}");
                        break;
                }
            }

            return sb.ToString().TrimEnd();
        }

        #endregion

    }

}
