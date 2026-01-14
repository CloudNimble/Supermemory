using CloudNimble.Supermemory.Models.Search;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to search operations in the Supermemory API.
    /// </summary>
    public class SearchResource : ResourceBase
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal SearchResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches documents using the v3 API.
        /// </summary>
        /// <param name="request">The search request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The search results.</returns>
        /// <remarks>
        /// <para>
        /// This method searches for documents based on semantic similarity to the query.
        /// Results are ranked by relevance score.
        /// </para>
        /// </remarks>
        public Task<SearchDocumentsResponse> SearchDocumentsAsync(
            SearchDocumentsRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                "/v3/search",
                request,
                SupermemoryJsonContext.Default.SearchDocumentsRequest,
                SupermemoryJsonContext.Default.SearchDocumentsResponse,
                cancellationToken);
        }

        /// <summary>
        /// Searches memories using the v4 API.
        /// </summary>
        /// <param name="request">The search request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The search results.</returns>
        /// <remarks>
        /// <para>
        /// The v4 search API provides enhanced memory search capabilities with
        /// support for different search modes and memory-specific filtering.
        /// </para>
        /// </remarks>
        public Task<SearchMemoriesResponse> SearchMemoriesAsync(
            SearchMemoriesRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                "/v4/search",
                request,
                SupermemoryJsonContext.Default.SearchMemoriesRequest,
                SupermemoryJsonContext.Default.SearchMemoriesResponse,
                cancellationToken);
        }

        /// <summary>
        /// Performs a simple search with just a query string.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="limit">The maximum number of results to return.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The search results.</returns>
        public Task<SearchDocumentsResponse> SearchAsync(
            string query,
            int limit = 10,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);

            var request = new SearchDocumentsRequest
            {
                Query = query,
                Limit = limit
            };

            return SearchDocumentsAsync(request, cancellationToken);
        }

        #endregion

    }

}
