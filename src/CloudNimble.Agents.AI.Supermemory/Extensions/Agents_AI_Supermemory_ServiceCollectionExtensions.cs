using System.Diagnostics.CodeAnalysis;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extension methods for configuring Supermemory Agent Framework services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class Agents_AI_Supermemory_ServiceCollectionExtensions
    {

        #region AddSupermemoryAgent

        /// <summary>
        /// Adds a Supermemory-enabled AI agent to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureAgent">Optional action to configure the agent options (applied after configuration binding).</param>
        /// <param name="configureContext">Optional action to configure the context provider options (applied after configuration binding).</param>
        /// <param name="configureHistory">Optional action to configure the history provider options (applied after configuration binding).</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method registers an AI agent that is fully configured with Supermemory integration.
        /// Configuration is automatically bound from the default configuration sections using AOT-compatible binding.
        /// All dependencies are resolved from DI:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="IChatClient"/> - The chat client for the agent</description></item>
        /// <item><description><see cref="SupermemoryClient"/> - The Supermemory client (register via <c>AddSupermemory()</c>)</description></item>
        /// <item><description><see cref="IOptions{ChatClientAgentOptions}"/> - Agent configuration from DI/config</description></item>
        /// <item><description><see cref="IOptions{SupermemoryContextProviderOptions}"/> - Context provider configuration</description></item>
        /// <item><description><see cref="IOptions{SupermemoryChatHistoryProviderOptions}"/> - History provider configuration</description></item>
        /// </list>
        /// <para>
        /// Example usage:
        /// <code>
        /// // In Program.cs
        /// builder.Services.AddSupermemory();
        /// builder.Services.AddSingleton&lt;IChatClient&gt;(sp =&gt;
        ///     sp.GetRequiredService&lt;AzureOpenAIClient&gt;()
        ///       .GetChatClient("gpt-4o-mini")
        ///       .AsIChatClient());
        /// builder.Services.AddSupermemoryAgent();
        ///
        /// // Configuration in appsettings.json:
        /// // {
        /// //   "Supermemory": { "ApiKey": "..." },
        /// //   "SupermemoryContext": { "DefaultContainerTag": "user-{sessionId}" },
        /// //   "SupermemoryHistory": { "MaxMessages": 50 }
        /// // }
        ///
        /// // Inject where needed
        /// public class MyService(ChatClientAgent agent) { }
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryAgent(
            this IServiceCollection services,
            Action<ChatClientAgentOptions>? configureAgent = null,
            Action<SupermemoryContextProviderOptions>? configureContext = null,
            Action<SupermemoryChatHistoryProviderOptions>? configureHistory = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Register default options if not already registered
            services.TryAddSingleton<IContainerTagResolver, DefaultContainerTagResolver>();

            // Bind context options from configuration, then apply programmatic configuration
            var contextOptionsBuilder = services.AddOptions<SupermemoryContextProviderOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(SupermemoryContextProviderOptions.SectionName);
                    options.LoadFrom(section);
                });

            if (configureContext is not null)
            {
                contextOptionsBuilder.Configure(configureContext);
            }

            // Bind history options from configuration, then apply programmatic configuration
            var historyOptionsBuilder = services.AddOptions<SupermemoryChatHistoryProviderOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(SupermemoryChatHistoryProviderOptions.SectionName);
                    options.LoadFrom(section);
                });

            if (configureHistory is not null)
            {
                historyOptionsBuilder.Configure(configureHistory);
            }

            // Register agent options with optional programmatic configuration
            var agentOptionsBuilder = services.AddOptions<ChatClientAgentOptions>();

            if (configureAgent is not null)
            {
                agentOptionsBuilder.Configure(configureAgent);
            }

            // Register the agent, resolving all dependencies from DI
            services.AddSingleton(sp =>
            {
                var chatClient = sp.GetRequiredService<IChatClient>();
                var supermemoryClient = sp.GetRequiredService<SupermemoryClient>();
                var agentOptions = sp.GetRequiredService<IOptions<ChatClientAgentOptions>>().Value;
                var contextOptions = sp.GetRequiredService<IOptions<SupermemoryContextProviderOptions>>().Value;
                var historyOptions = sp.GetRequiredService<IOptions<SupermemoryChatHistoryProviderOptions>>().Value;
                var containerTagResolver = sp.GetService<IContainerTagResolver>();

                // Configure the agent with Supermemory
                agentOptions.WithSupermemory(supermemoryClient, contextOptions, historyOptions, containerTagResolver);

                return chatClient.AsAIAgent(agentOptions);
            });

            return services;
        }

        /// <summary>
        /// Adds a Supermemory-enabled AI agent to the service collection with configuration binding.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="agentConfigSection">The configuration section name for agent options.</param>
        /// <param name="contextConfigSection">
        /// The configuration section name for context provider options.
        /// Defaults to <see cref="SupermemoryContextProviderOptions.SectionName"/>.
        /// </param>
        /// <param name="historyConfigSection">
        /// The configuration section name for history provider options.
        /// Defaults to <see cref="SupermemoryChatHistoryProviderOptions.SectionName"/>.
        /// </param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method binds configuration from the specified sections to the options classes.
        /// This overload is AOT-compatible and does not use reflection for configuration binding.
        /// </para>
        /// <para>
        /// Example appsettings.json:
        /// <code>
        /// {
        ///   "Supermemory": {
        ///     "ApiKey": "your-api-key"
        ///   },
        ///   "Agent": {
        ///     "Name": "MemoryAgent",
        ///     "Description": "An AI agent with persistent memory"
        ///   },
        ///   "SupermemoryContext": {
        ///     "DefaultContainerTag": "user-{sessionId}",
        ///     "SearchLimit": 5
        ///   },
        ///   "SupermemoryHistory": {
        ///     "DefaultContainerTag": "user-{sessionId}",
        ///     "MaxMessages": 50
        ///   }
        /// }
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryAgent(
            this IServiceCollection services,
            string agentConfigSection,
            string? contextConfigSection = null,
            string? historyConfigSection = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrWhiteSpace(agentConfigSection);

            // Use default section names if not specified
            contextConfigSection ??= SupermemoryContextProviderOptions.SectionName;
            historyConfigSection ??= SupermemoryChatHistoryProviderOptions.SectionName;

            // Bind options from configuration using AOT-compatible manual binding
            services.AddOptions<ChatClientAgentOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(agentConfigSection);
                    var name = section[nameof(ChatClientAgentOptions.Name)];
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        options.Name = name;
                    }

                    var description = section[nameof(ChatClientAgentOptions.Description)];
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        options.Description = description;
                    }
                });

            if (!string.IsNullOrWhiteSpace(contextConfigSection))
            {
                services.AddOptions<SupermemoryContextProviderOptions>()
                    .Configure<IConfiguration>((options, config) =>
                    {
                        var section = config.GetSection(contextConfigSection);
                        options.LoadFrom(section);
                    });
            }

            if (!string.IsNullOrWhiteSpace(historyConfigSection))
            {
                services.AddOptions<SupermemoryChatHistoryProviderOptions>()
                    .Configure<IConfiguration>((options, config) =>
                    {
                        var section = config.GetSection(historyConfigSection);
                        options.LoadFrom(section);
                    });
            }

            return services.AddSupermemoryAgent();
        }

        #endregion

        #region AddSupermemoryContextProvider

        /// <summary>
        /// Adds Supermemory context provider services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional action to configure the provider options (applied after configuration binding).</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// Configuration is automatically bound from the <see cref="SupermemoryContextProviderOptions.SectionName"/>
        /// section using AOT-compatible binding. The optional <paramref name="configure"/> action is applied
        /// after configuration binding, allowing programmatic overrides.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryContextProvider(
            this IServiceCollection services,
            Action<SupermemoryContextProviderOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IContainerTagResolver, DefaultContainerTagResolver>();

            // Bind options from configuration, then apply programmatic configuration
            var optionsBuilder = services.AddOptions<SupermemoryContextProviderOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(SupermemoryContextProviderOptions.SectionName);
                    options.LoadFrom(section);
                });

            if (configure is not null)
            {
                optionsBuilder.Configure(configure);
            }

            services.AddTransient(sp =>
            {
                var client = sp.GetRequiredService<SupermemoryClient>();
                var options = sp.GetRequiredService<IOptions<SupermemoryContextProviderOptions>>().Value;
                var resolver = sp.GetService<IContainerTagResolver>() ?? new DefaultContainerTagResolver();
                return new SupermemoryContextProvider(client, options, resolver);
            });

            return services;
        }

        #endregion

        #region AddSupermemoryChatHistoryProvider

        /// <summary>
        /// Adds Supermemory chat history provider services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional action to configure the provider options (applied after configuration binding).</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// Configuration is automatically bound from the <see cref="SupermemoryChatHistoryProviderOptions.SectionName"/>
        /// section using AOT-compatible binding. The optional <paramref name="configure"/> action is applied
        /// after configuration binding, allowing programmatic overrides.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryChatHistoryProvider(
            this IServiceCollection services,
            Action<SupermemoryChatHistoryProviderOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IContainerTagResolver, DefaultContainerTagResolver>();

            // Bind options from configuration, then apply programmatic configuration
            var optionsBuilder = services.AddOptions<SupermemoryChatHistoryProviderOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(SupermemoryChatHistoryProviderOptions.SectionName);
                    options.LoadFrom(section);
                });

            if (configure is not null)
            {
                optionsBuilder.Configure(configure);
            }

            services.AddTransient(sp =>
            {
                var client = sp.GetRequiredService<SupermemoryClient>();
                var options = sp.GetRequiredService<IOptions<SupermemoryChatHistoryProviderOptions>>().Value;
                var resolver = sp.GetService<IContainerTagResolver>() ?? new DefaultContainerTagResolver();
                return new SupermemoryChatHistoryProvider(client, options, resolver);
            });

            return services;
        }

        #endregion

        #region AddSupermemoryAgentFramework

        /// <summary>
        /// Adds both Supermemory providers for full Agent Framework integration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureContext">Optional action to configure context provider options.</param>
        /// <param name="configureHistory">Optional action to configure history provider options.</param>
        /// <returns>The service collection for chaining.</returns>
        [Obsolete("Use AddSupermemoryAgent() instead for full DI integration.")]
        public static IServiceCollection AddSupermemoryAgentFramework(
            this IServiceCollection services,
            Action<SupermemoryContextProviderOptions>? configureContext = null,
            Action<SupermemoryChatHistoryProviderOptions>? configureHistory = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSupermemoryContextProvider(configureContext);
            services.AddSupermemoryChatHistoryProvider(configureHistory);

            return services;
        }

        #endregion

        #region AddContainerTagResolver

        /// <summary>
        /// Adds a custom container tag resolver to the service collection.
        /// </summary>
        /// <typeparam name="T">The resolver implementation type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddContainerTagResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
            where T : class, IContainerTagResolver
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<IContainerTagResolver, T>();

            return services;
        }

        #endregion

    }

}
