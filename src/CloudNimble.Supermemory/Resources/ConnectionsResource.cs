using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Connections;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to connection management operations in the Supermemory API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Connections allow you to integrate external data sources like Notion, Google Drive,
    /// OneDrive, and GitHub with Supermemory.
    /// </para>
    /// </remarks>
    public class ConnectionsResource : ResourceBase
    {

        #region Constants

        private const string BasePath = "/v3/connections";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionsResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal ConnectionsResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new connection for the specified provider.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <param name="request">The create connection request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the connection details or redirect URL.</returns>
        public Task<CreateConnectionResponse> CreateAsync(
            ConnectionProvider provider,
            CreateConnectionRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var providerString = GetProviderString(provider);
            return PostAsync(
                $"{BasePath}/{providerString}",
                request,
                SupermemoryJsonContext.Default.CreateConnectionRequest,
                SupermemoryJsonContext.Default.CreateConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Lists all connections with optional filtering.
        /// </summary>
        /// <param name="request">The list connections request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of connections.</returns>
        public Task<List<ConnectionResponse>> ListAsync(
            ListConnectionsRequest? request = null,
            CancellationToken cancellationToken = default)
        {
            var body = request ?? new ListConnectionsRequest();
            return PostAsync(
                $"{BasePath}/list",
                body,
                SupermemoryJsonContext.Default.ListConnectionsRequest,
                SupermemoryJsonContext.Default.ListConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets a connection by its ID.
        /// </summary>
        /// <param name="connectionId">The connection ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The connection details.</returns>
        public Task<ConnectionResponse> GetAsync(
            string connectionId,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);
            return GetAsync(
                $"{BasePath}/{Uri.EscapeDataString(connectionId)}",
                SupermemoryJsonContext.Default.ConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Configures an existing connection.
        /// </summary>
        /// <param name="connectionId">The connection ID.</param>
        /// <param name="request">The configure connection request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating the configuration result.</returns>
        public Task<ConfigureConnectionResponse> ConfigureAsync(
            string connectionId,
            ConfigureConnectionRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                $"{BasePath}/{Uri.EscapeDataString(connectionId)}/configure",
                request,
                SupermemoryJsonContext.Default.ConfigureConnectionRequest,
                SupermemoryJsonContext.Default.ConfigureConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a connection by its ID.
        /// </summary>
        /// <param name="connectionId">The connection ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating the deletion result.</returns>
        public Task<DeleteConnectionResponse> DeleteAsync(
            string connectionId,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);
            return DeleteAsync(
                $"{BasePath}/{Uri.EscapeDataString(connectionId)}",
                SupermemoryJsonContext.Default.DeleteConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Deletes all connections for a specific provider.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating the deletion result.</returns>
        public Task<DeleteConnectionResponse> DeleteByProviderAsync(
            ConnectionProvider provider,
            CancellationToken cancellationToken = default)
        {
            var providerString = GetProviderString(provider);
            return DeleteAsync(
                $"{BasePath}/{providerString}",
                SupermemoryJsonContext.Default.DeleteConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets connection information by container tag.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <param name="request">The request containing the container tag.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The connection response.</returns>
        public Task<ConnectionResponse> GetByTagAsync(
            ConnectionProvider provider,
            GetByTagRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var providerString = GetProviderString(provider);
            return PostAsync(
                $"{BasePath}/{providerString}/connection",
                request,
                SupermemoryJsonContext.Default.GetByTagRequest,
                SupermemoryJsonContext.Default.ConnectionResponse,
                cancellationToken);
        }

        /// <summary>
        /// Imports resources from a provider connection.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <param name="request">The import request containing resources to import.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A status response indicating the import was initiated.</returns>
        public Task<StatusResponse> ImportAsync(
            ConnectionProvider provider,
            List<ConnectionResource> request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var providerString = GetProviderString(provider);
            return PostAsync(
                $"{BasePath}/{providerString}/import",
                request,
                SupermemoryJsonContext.Default.ListConnectionResource,
                SupermemoryJsonContext.Default.StatusResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets documents from a provider connection.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <param name="request">The request containing the container tag.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of connection resources.</returns>
        public Task<ConnectionResourcesResponse> GetDocumentsAsync(
            ConnectionProvider provider,
            GetByTagRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var providerString = GetProviderString(provider);
            return PostAsync(
                $"{BasePath}/{providerString}/documents",
                request,
                SupermemoryJsonContext.Default.GetByTagRequest,
                SupermemoryJsonContext.Default.ConnectionResourcesResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets resources available from a connection.
        /// </summary>
        /// <param name="connectionId">The connection ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of available resources.</returns>
        public Task<ConnectionResourcesResponse> GetResourcesAsync(
            string connectionId,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);
            return GetAsync(
                $"{BasePath}/{Uri.EscapeDataString(connectionId)}/resources",
                SupermemoryJsonContext.Default.ConnectionResourcesResponse,
                cancellationToken);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the string representation of a connection provider for use in API paths.
        /// </summary>
        /// <param name="provider">The connection provider.</param>
        /// <returns>The provider string.</returns>
        private static string GetProviderString(ConnectionProvider provider)
        {
            return provider switch
            {
                ConnectionProvider.Notion => "notion",
                ConnectionProvider.GoogleDrive => "google_drive",
                ConnectionProvider.OneDrive => "onedrive",
                ConnectionProvider.GitHub => "github",
                _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unknown connection provider.")
            };
        }

        #endregion

    }

}
