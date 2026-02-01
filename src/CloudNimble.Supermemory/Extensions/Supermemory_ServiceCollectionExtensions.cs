using System.Net.Http.Headers;
using CloudNimble.Supermemory;
using CloudNimble.Supermemory.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extension methods for registering Supermemory services with dependency injection.
    /// </summary>
    public static class Supermemory_ServiceCollectionExtensions
    {

        #region Constants

        /// <summary>
        /// The User-Agent string sent with all HTTP requests.
        /// </summary>
        /// <remarks>
        /// Version is populated at compile time from the assembly version for AOT compatibility.
        /// </remarks>
        private static readonly string UserAgent = $"CloudNimble.Supermemory/{VersionInfo.Version}";

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds Supermemory services to the service collection using configuration from the default section.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the <see cref="SupermemoryClient"/> using the .NET Options pattern.
        /// Configuration is read from the <see cref="SupermemoryClientOptions.SectionName"/> section.
        /// </para>
        /// <para>
        /// This overload is AOT-compatible and does not use reflection for configuration binding.
        /// </para>
        /// <para>
        /// Example <c>appsettings.json</c>:
        /// <code>
        /// {
        ///   "Supermemory": {
        ///     "ApiKey": "your-api-key",
        ///     "BaseUrl": "https://api.supermemory.ai",
        ///     "Timeout": "00:01:00",
        ///     "MaxRetries": 2
        ///   }
        /// }
        /// </code>
        /// </para>
        /// <para>
        /// Example usage:
        /// <code>
        /// // In Program.cs
        /// builder.Services.AddSupermemory();
        ///
        /// // Inject where needed
        /// public class MyService(SupermemoryClient supermemory) { }
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemory(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            return AddSupermemoryCore(services, SupermemoryClientOptions.SectionName, null);
        }

        /// <summary>
        /// Adds Supermemory services to the service collection with programmatic configuration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">An action to configure the <see cref="SupermemoryClientOptions"/>.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This overload is AOT-compatible and does not require reflection.
        /// Use this method when you need to configure options programmatically.
        /// </para>
        /// <para>
        /// Example usage:
        /// <code>
        /// builder.Services.AddSupermemory(options =>
        /// {
        ///     options.ApiKey = Environment.GetEnvironmentVariable("SUPERMEMORY_API_KEY");
        ///     options.Timeout = TimeSpan.FromSeconds(120);
        ///     options.MaxRetries = 5;
        /// });
        /// </code>
        /// </para>
        /// <para>
        /// To load from configuration in an AOT-compatible way:
        /// <code>
        /// builder.Services.AddSupermemory(options =>
        /// {
        ///     options.LoadFrom(builder.Configuration.GetSection(SupermemoryClientOptions.SectionName));
        /// });
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemory(
            this IServiceCollection services,
            Action<SupermemoryClientOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configure);

            return AddSupermemoryCore(services, null, configure);
        }

        /// <summary>
        /// Adds Supermemory services to the service collection with both configuration binding and programmatic configuration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="sectionName">
        /// The name of the configuration section to bind options from.
        /// Use <see cref="SupermemoryClientOptions.SectionName"/> for the default section name.
        /// </param>
        /// <param name="configure">An action to configure the <see cref="SupermemoryClientOptions"/>. This is applied after configuration binding.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This overload allows combining configuration file settings with programmatic overrides.
        /// The <paramref name="configure"/> action is applied after binding from configuration,
        /// allowing you to override specific values while keeping others from configuration.
        /// </para>
        /// <para>
        /// This overload is AOT-compatible and does not use reflection for configuration binding.
        /// </para>
        /// <para>
        /// Example usage:
        /// <code>
        /// // ApiKey from config, override timeout programmatically
        /// builder.Services.AddSupermemory(SupermemoryClientOptions.SectionName, options =>
        /// {
        ///     options.Timeout = TimeSpan.FromSeconds(120);
        /// });
        /// </code>
        /// </para>
        /// </remarks>
        public static IServiceCollection AddSupermemory(
            this IServiceCollection services,
            string sectionName,
            Action<SupermemoryClientOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
            ArgumentNullException.ThrowIfNull(configure);

            return AddSupermemoryCore(services, sectionName, configure);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Core implementation for adding Supermemory services.
        /// </summary>
        private static IServiceCollection AddSupermemoryCore(
            IServiceCollection services,
            string? sectionName,
            Action<SupermemoryClientOptions>? configure)
        {
            var optionsBuilder = services.AddOptions<SupermemoryClientOptions>();

            // First, bind from configuration if a section is specified
            if (!string.IsNullOrWhiteSpace(sectionName))
            {
                optionsBuilder.Configure<IConfiguration>((options, config) =>
                {
                    var section = config.GetSection(sectionName);
                    options.LoadFrom(section);
                });
            }

            // Then, apply programmatic configuration if provided
            if (configure is not null)
            {
                optionsBuilder.Configure(configure);
            }

            // Configure typed HttpClient via IHttpClientFactory
            services.AddHttpClient<SupermemoryClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<SupermemoryClientOptions>>().Value;
                ConfigureHttpClient(client, options);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10
            });

            return services;
        }

        /// <summary>
        /// Configures the HttpClient with required headers and settings.
        /// </summary>
        /// <param name="client">The HttpClient to configure.</param>
        /// <param name="options">The client options.</param>
        private static void ConfigureHttpClient(HttpClient client, SupermemoryClientOptions options)
        {
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = options.Timeout;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
            }

            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        }

        #endregion

    }

}
