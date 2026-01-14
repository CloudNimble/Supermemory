using CloudNimble.Supermemory.Models.Memories;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to memory management operations in the Supermemory API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Memories are the core abstraction in Supermemory, representing pieces of information
    /// that can be stored, searched, and retrieved.
    /// </para>
    /// </remarks>
    public class MemoriesResource : ResourceBase
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoriesResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal MemoriesResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Forgets (deletes) memories based on the specified criteria.
        /// </summary>
        /// <param name="request">The forget memory request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating which memories were forgotten.</returns>
        /// <remarks>
        /// <para>
        /// This operation permanently removes memories that match the specified criteria.
        /// Use with caution as this cannot be undone.
        /// </para>
        /// </remarks>
        public Task<ForgetMemoryResponse> ForgetAsync(
            ForgetMemoryRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return DeleteAsync(
                "/v4/memories",
                request,
                SupermemoryJsonContext.Default.ForgetMemoryRequest,
                SupermemoryJsonContext.Default.ForgetMemoryResponse,
                cancellationToken);
        }

        /// <summary>
        /// Updates existing memories based on the specified criteria.
        /// </summary>
        /// <param name="request">The update memory request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating which memories were updated.</returns>
        public Task<UpdateMemoryResponse> UpdateAsync(
            UpdateMemoryRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PatchAsync(
                "/v4/memories",
                request,
                SupermemoryJsonContext.Default.UpdateMemoryRequest,
                SupermemoryJsonContext.Default.UpdateMemoryResponse,
                cancellationToken);
        }

        #endregion

    }

}
