namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Interface for chat history reduction strategies.
    /// </summary>
    public interface IChatReducer
    {
        /// <summary>
        /// Reduces the chat history to a manageable size.
        /// </summary>
        /// <param name="messages">The current messages.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The reduced set of messages.</returns>
        ValueTask<IEnumerable<ChatMessageWrapper>> ReduceAsync(
            IEnumerable<ChatMessageWrapper> messages,
            CancellationToken cancellationToken = default);
    }

}
