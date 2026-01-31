using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;


namespace Microsoft.Agents.AI
{

    /// <summary>
    /// Extension methods for configuring <see cref="ChatClientAgentOptions"/> with Supermemory providers.
    /// </summary>
    public static class Agents_AI_Supermemory_AgentOptionsExtensions
    {

        /// <summary>
        /// Configures the agent to use Supermemory for semantic memory.
        /// </summary>
        /// <param name="options">The agent options to configure.</param>
        /// <param name="client">The Supermemory client.</param>
        /// <param name="providerOptions">Optional context provider configuration.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <returns>The configured options for chaining.</returns>
        public static ChatClientAgentOptions WithSupermemoryContext(
            this ChatClientAgentOptions options,
            SupermemoryClient client,
            SupermemoryContextProviderOptions? providerOptions = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(client);

            options.AIContextProviderFactory = (ctx, ct) =>
            {
                var provider = ctx.SerializedState.ValueKind == System.Text.Json.JsonValueKind.Object
                    ? new SupermemoryContextProvider(client, ctx.SerializedState, ctx.JsonSerializerOptions, providerOptions, containerTagResolver)
                    : new SupermemoryContextProvider(client, providerOptions, containerTagResolver);

                return ValueTask.FromResult<AIContextProvider>(provider);
            };

            return options;
        }

        /// <summary>
        /// Configures the agent to use Supermemory for chat history storage.
        /// </summary>
        /// <param name="options">The agent options to configure.</param>
        /// <param name="client">The Supermemory client.</param>
        /// <param name="providerOptions">Optional chat history provider configuration.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <returns>The configured options for chaining.</returns>
        public static ChatClientAgentOptions WithSupermemoryHistory(
            this ChatClientAgentOptions options,
            SupermemoryClient client,
            SupermemoryChatHistoryProviderOptions? providerOptions = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(client);

            options.ChatHistoryProviderFactory = (ctx, ct) =>
            {
                var provider = ctx.SerializedState.ValueKind == System.Text.Json.JsonValueKind.Object
                    ? new SupermemoryChatHistoryProvider(client, ctx.SerializedState, ctx.JsonSerializerOptions, providerOptions, containerTagResolver)
                    : new SupermemoryChatHistoryProvider(client, providerOptions, containerTagResolver);

                return ValueTask.FromResult<ChatHistoryProvider>(provider);
            };

            return options;
        }

        /// <summary>
        /// Configures the agent to use Supermemory for both memory and history.
        /// </summary>
        /// <param name="options">The agent options to configure.</param>
        /// <param name="client">The Supermemory client.</param>
        /// <param name="contextOptions">Optional context provider configuration.</param>
        /// <param name="historyOptions">Optional chat history provider configuration.</param>
        /// <param name="containerTagResolver">Optional custom container tag resolver.</param>
        /// <returns>The configured options for chaining.</returns>
        public static ChatClientAgentOptions WithSupermemory(
            this ChatClientAgentOptions options,
            SupermemoryClient client,
            SupermemoryContextProviderOptions? contextOptions = null,
            SupermemoryChatHistoryProviderOptions? historyOptions = null,
            IContainerTagResolver? containerTagResolver = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(client);

            options.WithSupermemoryContext(client, contextOptions, containerTagResolver);
            options.WithSupermemoryHistory(client, historyOptions, containerTagResolver);

            return options;
        }

    }

}
