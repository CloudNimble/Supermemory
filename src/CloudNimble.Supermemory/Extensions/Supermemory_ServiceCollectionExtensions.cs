using CloudNimble.Supermemory;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extension methods for registering Supermemory services with dependency injection.
    /// </summary>
    public static class Supermemory_ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the Supermemory client to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// Example usage:
        /// <code>
        /// services.AddSupermemoryClient("your-api-key");
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryClient(
            this IServiceCollection services,
            string apiKey)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

            return services.AddSupermemoryClient(options =>
            {
                options.ApiKey = apiKey;
            });
        }

        /// <summary>
        /// Adds the Supermemory client to the service collection with configuration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">The action to configure client options.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// Example usage:
        /// <code>
        /// services.AddSupermemoryClient(options =>
        /// {
        ///     options.ApiKey = "your-api-key";
        ///     options.Timeout = TimeSpan.FromSeconds(120);
        ///     options.MaxRetries = 3;
        /// });
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryClient(
            this IServiceCollection services,
            Action<SupermemoryClientOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            services.AddSingleton(sp =>
            {
                var options = new SupermemoryClientOptions();
                configure(options);
                return new SupermemoryClient(options);
            });

            return services;
        }

        /// <summary>
        /// Adds the Supermemory client to the service collection using environment variables.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the client using the following environment variables:
        /// <list type="bullet">
        /// <item><description>SUPERMEMORY_API_KEY: The API key for authentication</description></item>
        /// <item><description>SUPERMEMORY_BASE_URL: The base URL (optional, defaults to https://api.supermemory.ai)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryClientFromEnvironment(
            this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var options = SupermemoryClientOptions.FromEnvironment();
                return new SupermemoryClient(options);
            });

            return services;
        }

        /// <summary>
        /// Adds the Supermemory client to the service collection using IHttpClientFactory.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">The action to configure client options.</param>
        /// <param name="configureHttpClient">Optional action to configure the HttpClient.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method uses IHttpClientFactory for better HttpClient lifecycle management.
        /// This is the recommended approach for ASP.NET Core applications.
        /// </para>
        /// <para>
        /// Example usage:
        /// <code>
        /// services.AddSupermemoryClientWithFactory(
        ///     options => options.ApiKey = "your-api-key",
        ///     client => client.Timeout = TimeSpan.FromSeconds(120));
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemoryClientWithFactory(
            this IServiceCollection services,
            Action<SupermemoryClientOptions> configure,
            Action<HttpClient>? configureHttpClient = null)
        {
            ArgumentNullException.ThrowIfNull(configure);

            var httpClientBuilder = services.AddHttpClient<SupermemoryClient>((sp, client) =>
            {
                configureHttpClient?.Invoke(client);
            });

            services.AddSingleton(sp =>
            {
                var options = new SupermemoryClientOptions();
                configure(options);

                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = factory.CreateClient(nameof(SupermemoryClient));
                options.HttpClient = httpClient;

                return new SupermemoryClient(options);
            });

            return services;
        }

    }

}
