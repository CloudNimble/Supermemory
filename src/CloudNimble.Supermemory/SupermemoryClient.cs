using CloudNimble.Supermemory.Resources;
using Microsoft.Extensions.Options;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// The main client for interacting with the Supermemory API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This client provides access to all Supermemory API resources through typed resource properties.
    /// It is designed to be used with dependency injection and the <see cref="IOptions{TOptions}"/> pattern.
    /// </para>
    /// <para>
    /// Example usage with dependency injection:
    /// <code>
    /// // In Program.cs
    /// builder.Services.AddSupermemory();
    ///
    /// // In your service
    /// public class MyService(SupermemoryClient supermemory)
    /// {
    ///     public async Task SearchAsync(string query)
    ///     {
    ///         var results = await supermemory.Search.SearchDocumentsAsync(
    ///             new SearchDocumentsRequest { Query = query });
    ///     }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public sealed class SupermemoryClient : IDisposable
    {

        #region Private Members

        private readonly HttpClient _httpClient;
        private readonly SupermemoryClientOptions _options;

        private DocumentsResource? _documents;
        private SearchResource? _search;
        private MemoriesResource? _memories;
        private ConnectionsResource? _connections;
        private SettingsResource? _settings;
        private ProfileResource? _profile;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Documents resource for managing documents.
        /// </summary>
        public DocumentsResource Documents => _documents ??= new DocumentsResource(_httpClient, _options);

        /// <summary>
        /// Gets the Search resource for searching documents and memories.
        /// </summary>
        public SearchResource Search => _search ??= new SearchResource(_httpClient, _options);

        /// <summary>
        /// Gets the Memories resource for managing memories.
        /// </summary>
        public MemoriesResource Memories => _memories ??= new MemoriesResource(_httpClient, _options);

        /// <summary>
        /// Gets the Connections resource for managing external connections.
        /// </summary>
        public ConnectionsResource Connections => _connections ??= new ConnectionsResource(_httpClient, _options);

        /// <summary>
        /// Gets the Settings resource for managing organization settings.
        /// </summary>
        public SettingsResource Settings => _settings ??= new SettingsResource(_httpClient, _options);

        /// <summary>
        /// Gets the Profile resource for managing user profiles.
        /// </summary>
        public ProfileResource Profile => _profile ??= new ProfileResource(_httpClient, _options);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryClient"/> class.
        /// </summary>
        /// <param name="httpClient">
        /// The <see cref="HttpClient"/> configured by <see cref="IHttpClientFactory"/>.
        /// This client is pre-configured with base address, timeout, and authorization headers.
        /// </param>
        /// <param name="options">
        /// The configuration options for the client, typically bound from the "Supermemory" configuration section.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the options fail validation.</exception>
        /// <remarks>
        /// <para>
        /// This constructor is designed to be used with the typed client pattern via <see cref="IHttpClientFactory"/>.
        /// The <see cref="HttpClient"/> lifecycle is managed by the factory and should not be disposed manually.
        /// </para>
        /// <para>
        /// Register the client using the <c>AddSupermemory()</c> extension method:
        /// <code>
        /// builder.Services.AddSupermemory();
        /// </code>
        /// </para>
        /// </remarks>
        public SupermemoryClient(HttpClient httpClient, IOptions<SupermemoryClientOptions> options)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(options);

            _options = options.Value;
            _options.Validate();
            _httpClient = httpClient;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Disposes of the client. This is a no-op when using <see cref="IHttpClientFactory"/>
        /// since the <see cref="HttpClient"/> lifecycle is managed by the factory.
        /// </summary>
        public void Dispose()
        {
            // No-op: HttpClient lifecycle is managed by IHttpClientFactory
        }

        #endregion

    }

}
