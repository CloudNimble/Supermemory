using CloudNimble.Supermemory.Models.Settings;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to settings management operations in the Supermemory API.
    /// </summary>
    public class SettingsResource : ResourceBase
    {

        #region Constants

        private const string BasePath = "/v3/settings";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal SettingsResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the current organization settings.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The current settings.</returns>
        public Task<SettingsResponse> GetAsync(CancellationToken cancellationToken = default)
        {
            return GetAsync(BasePath, SupermemoryJsonContext.Default.SettingsResponse, cancellationToken);
        }

        /// <summary>
        /// Updates the organization settings.
        /// </summary>
        /// <param name="request">The settings update request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated settings response.</returns>
        public Task<UpdateSettingsResponse> UpdateAsync(
            UpdateSettingsRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PatchAsync(
                BasePath,
                request,
                SupermemoryJsonContext.Default.UpdateSettingsRequest,
                SupermemoryJsonContext.Default.UpdateSettingsResponse,
                cancellationToken);
        }

        #endregion

    }

}
