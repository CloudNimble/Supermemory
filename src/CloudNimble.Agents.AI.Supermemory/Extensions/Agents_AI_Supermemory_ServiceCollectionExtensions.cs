using System.Diagnostics.CodeAnalysis;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extension methods for configuring Supermemory Agent Framework services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class Agents_AI_Supermemory_ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds Supermemory context provider services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional action to configure the provider options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSupermemoryContextProvider(
            this IServiceCollection services,
            Action<SupermemoryContextProviderOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IContainerTagResolver, DefaultContainerTagResolver>();

            // Register options with optional configuration
            services.AddSingleton(sp =>
            {
                var options = new SupermemoryContextProviderOptions();
                configure?.Invoke(options);
                return options;
            });

            services.AddTransient(sp =>
            {
                var client = sp.GetRequiredService<SupermemoryClient>();
                var options = sp.GetRequiredService<SupermemoryContextProviderOptions>();
                var resolver = sp.GetService<IContainerTagResolver>() ?? new DefaultContainerTagResolver();
                return new SupermemoryContextProvider(client, options, resolver);
            });

            return services;
        }

        /// <summary>
        /// Adds Supermemory chat history provider services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional action to configure the provider options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSupermemoryChatHistoryProvider(
            this IServiceCollection services,
            Action<SupermemoryChatHistoryProviderOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IContainerTagResolver, DefaultContainerTagResolver>();

            // Register options with optional configuration
            services.AddSingleton(sp =>
            {
                var options = new SupermemoryChatHistoryProviderOptions();
                configure?.Invoke(options);
                return options;
            });

            services.AddTransient(sp =>
            {
                var client = sp.GetRequiredService<SupermemoryClient>();
                var options = sp.GetRequiredService<SupermemoryChatHistoryProviderOptions>();
                var resolver = sp.GetService<IContainerTagResolver>() ?? new DefaultContainerTagResolver();
                return new SupermemoryChatHistoryProvider(client, options, resolver);
            });

            return services;
        }

        /// <summary>
        /// Adds both Supermemory providers for full Agent Framework integration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureContext">Optional action to configure context provider options.</param>
        /// <param name="configureHistory">Optional action to configure history provider options.</param>
        /// <returns>The service collection for chaining.</returns>
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

    }

}
