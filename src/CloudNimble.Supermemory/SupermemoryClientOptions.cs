using Microsoft.Extensions.Configuration;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryClient"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is designed to be used with the .NET Options pattern for configuration binding.
    /// Configuration can come from any .NET configuration source:
    /// </para>
    /// <list type="bullet">
    /// <item><description><c>appsettings.json</c> / <c>appsettings.{Environment}.json</c></description></item>
    /// <item><description>Environment variables (e.g., <c>Supermemory__ApiKey</c>)</description></item>
    /// <item><description>User secrets (for local development)</description></item>
    /// <item><description>Azure Key Vault, AWS Secrets Manager, etc.</description></item>
    /// <item><description>Command line arguments</description></item>
    /// </list>
    /// <para>
    /// Example configuration in <c>appsettings.json</c>:
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
    /// </remarks>
    public class SupermemoryClientOptions
    {

        #region Constants

        /// <summary>
        /// The default configuration section name for <see cref="SupermemoryClientOptions"/>.
        /// </summary>
        public const string SectionName = "Supermemory";

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
        /// </summary>
        /// <remarks>
        /// This is required for authenticating with the Supermemory API.
        /// The API key can be configured via any .NET configuration source,
        /// such as environment variables (e.g., <c>Supermemory__ApiKey</c>),
        /// user secrets, or <c>appsettings.json</c>.
        /// </remarks>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the base URL for the Supermemory API.
        /// Defaults to <see cref="DefaultBaseUrl"/>.
        /// </summary>
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads configuration values from the specified section into this options instance.
        /// This method is AOT-compatible and does not use reflection.
        /// </summary>
        /// <param name="section">The configuration section to load from.</param>
        public void LoadFrom(IConfigurationSection section)
        {
            ArgumentNullException.ThrowIfNull(section);

            var apiKey = section[nameof(ApiKey)];
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                ApiKey = apiKey;
            }

            var baseUrl = section[nameof(BaseUrl)];
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                BaseUrl = baseUrl;
            }

            var timeoutStr = section[nameof(Timeout)];
            if (!string.IsNullOrWhiteSpace(timeoutStr) && TimeSpan.TryParse(timeoutStr, out var timeout))
            {
                Timeout = timeout;
            }

            var maxRetriesStr = section[nameof(MaxRetries)];
            if (!string.IsNullOrWhiteSpace(maxRetriesStr) && int.TryParse(maxRetriesStr, out var maxRetries))
            {
                MaxRetries = maxRetries;
            }
        }

        /// <summary>
        /// Validates the options and throws if invalid.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when <see cref="ApiKey"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when <see cref="BaseUrl"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <see cref="Timeout"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <see cref="MaxRetries"/> is negative.</exception>
        public void Validate()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ApiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(BaseUrl);

            if (Timeout <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(Timeout), Timeout, "The Timeout must be greater than zero.");
            }

            if (MaxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxRetries), MaxRetries, "The MaxRetries cannot be negative.");
            }
        }

        #endregion

    }

}
