namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryClient"/>.
    /// </summary>
    public class SupermemoryClientOptions
    {

        #region Constants

        /// <summary>
        /// The default base URL for the Supermemory API.
        /// </summary>
        public const string DefaultBaseUrl = "https://api.supermemory.ai";

        /// <summary>
        /// The default timeout in seconds.
        /// </summary>
        public const int DefaultTimeoutSeconds = 60;

        /// <summary>
        /// The default maximum number of retries.
        /// </summary>
        public const int DefaultMaxRetries = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the API key for authentication.
        /// This is required unless an <see cref="HttpClient"/> with pre-configured authorization is provided.
        /// </summary>
        /// <remarks>
        /// The API key can also be set via the SUPERMEMORY_API_KEY environment variable.
        /// </remarks>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the base URL for the Supermemory API.
        /// Defaults to <see cref="DefaultBaseUrl"/>.
        /// </summary>
        /// <remarks>
        /// The base URL can also be set via the SUPERMEMORY_BASE_URL environment variable.
        /// </remarks>
        public string BaseUrl { get; set; } = DefaultBaseUrl;

        /// <summary>
        /// Gets or sets the timeout for HTTP requests.
        /// Defaults to 60 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(DefaultTimeoutSeconds);

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed requests.
        /// Defaults to 2.
        /// </summary>
        public int MaxRetries { get; set; } = DefaultMaxRetries;

        /// <summary>
        /// Gets or sets a custom <see cref="HttpClient"/> to use for requests.
        /// If not provided, a new client will be created internally.
        /// </summary>
        /// <remarks>
        /// When providing a custom HttpClient, ensure it has proper lifetime management
        /// and is configured with appropriate handlers for connection pooling.
        /// </remarks>
        public HttpClient? HttpClient { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryClientOptions"/> class.
        /// </summary>
        public SupermemoryClientOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryClientOptions"/> class with an API key.
        /// </summary>
        /// <param name="apiKey">The API key for authentication.</param>
        public SupermemoryClientOptions(string apiKey)
        {
            ApiKey = apiKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates options from environment variables.
        /// </summary>
        /// <returns>A new <see cref="SupermemoryClientOptions"/> instance configured from environment variables.</returns>
        public static SupermemoryClientOptions FromEnvironment()
        {
            var options = new SupermemoryClientOptions
            {
                ApiKey = Environment.GetEnvironmentVariable("SUPERMEMORY_API_KEY")
            };

            var baseUrl = Environment.GetEnvironmentVariable("SUPERMEMORY_BASE_URL");
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                options.BaseUrl = baseUrl;
            }

            return options;
        }

        /// <summary>
        /// Validates the options and throws if invalid.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required options are missing or invalid.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiKey) && HttpClient is null)
            {
                throw new InvalidOperationException(
                    "The API key is required. Either provide an ApiKey or a pre-configured HttpClient. " +
                    "You can also set the SUPERMEMORY_API_KEY environment variable.");
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new InvalidOperationException("The BaseUrl cannot be null or empty.");
            }

            if (Timeout <= TimeSpan.Zero)
            {
                throw new InvalidOperationException("The Timeout must be greater than zero.");
            }

            if (MaxRetries < 0)
            {
                throw new InvalidOperationException("The MaxRetries cannot be negative.");
            }
        }

        #endregion

    }

}
