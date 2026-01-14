using CloudNimble.Supermemory.Models.Profile;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to profile operations in the Supermemory API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Profiles provide personalized context based on stored memories,
    /// enabling more relevant and contextual search results.
    /// </para>
    /// </remarks>
    public class ProfileResource : ResourceBase
    {

        #region Constants

        private const string BasePath = "/v4/profile";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal ProfileResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a profile based on the specified criteria.
        /// </summary>
        /// <param name="request">The profile request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The profile response containing dynamic and static profile data.</returns>
        /// <remarks>
        /// <para>
        /// The profile contains two types of data:
        /// <list type="bullet">
        /// <item><description>Dynamic: Recent memories that are contextually relevant</description></item>
        /// <item><description>Static: Long-term relevant information</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public Task<ProfileResponse> GetAsync(
            ProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                BasePath,
                request,
                SupermemoryJsonContext.Default.ProfileRequest,
                SupermemoryJsonContext.Default.ProfileResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets a profile for a specific container tag.
        /// </summary>
        /// <param name="containerTag">The container tag to filter the profile.</param>
        /// <param name="query">Optional search query to include relevant results.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The profile response.</returns>
        public Task<ProfileResponse> GetAsync(
            string containerTag,
            string? query = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(containerTag);

            var request = new ProfileRequest
            {
                ContainerTag = containerTag,
                Query = query
            };

            return GetAsync(request, cancellationToken);
        }

        #endregion

    }

}
