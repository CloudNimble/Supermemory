using System.Net.Http.Headers;
using CloudNimble.Supermemory.Resources;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// The main client for interacting with the Supermemory API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This client provides access to all Supermemory API resources through typed resource properties.
    /// </para>
    /// <para>
    /// Example usage:
    /// <code>
    /// using var client = new SupermemoryClient("your-api-key");
    /// var response = await client.Documents.AddAsync(new AddDocumentRequest
    /// {
    ///     Content = "https://example.com",
    ///     ContainerTags = ["my-container"]
    /// });
    /// </code>
    /// </para>
    /// </remarks>
    public sealed class SupermemoryClient : IDisposable
    {

        #region Private Members

        private readonly HttpClient _httpClient;
        private readonly SupermemoryClientOptions _options;
        private readonly bool _ownsHttpClient;
        private bool _disposed;

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
        /// Initializes a new instance of the <see cref="SupermemoryClient"/> class with an API key.
        /// </summary>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="apiKey"/> is null or empty.</exception>
        public SupermemoryClient(string apiKey)
            : this(new SupermemoryClientOptions(apiKey))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryClient"/> class with options.
        /// </summary>
        /// <param name="options">The client options.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the options are invalid.</exception>
        public SupermemoryClient(SupermemoryClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _options.Validate();

            if (_options.HttpClient != null)
            {
                _httpClient = _options.HttpClient;
                _ownsHttpClient = false;
            }
            else
            {
                _httpClient = CreateHttpClient(_options);
                _ownsHttpClient = true;
            }

            ConfigureHttpClient(_httpClient, _options);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new HttpClient with optimal settings for API communication.
        /// </summary>
        /// <param name="options">The client options.</param>
        /// <returns>A configured HttpClient instance.</returns>
        private static HttpClient CreateHttpClient(SupermemoryClientOptions options)
        {
            // Use SocketsHttpHandler for efficient connection pooling
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10
            };

            return new HttpClient(handler)
            {
                Timeout = options.Timeout
            };
        }

        /// <summary>
        /// Configures the HttpClient with required headers.
        /// </summary>
        /// <param name="httpClient">The HttpClient to configure.</param>
        /// <param name="options">The client options.</param>
        private static void ConfigureHttpClient(HttpClient httpClient, SupermemoryClientOptions options)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
            }

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CloudNimble.Supermemory/1.0.0");
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Disposes of the client and its resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_ownsHttpClient)
            {
                _httpClient.Dispose();
            }

            _disposed = true;
        }

        #endregion

    }

}
