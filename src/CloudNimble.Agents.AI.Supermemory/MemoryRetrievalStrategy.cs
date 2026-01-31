namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Specifies the strategy for retrieving memories from Supermemory.
    /// </summary>
    public enum MemoryRetrievalStrategy
    {
        /// <summary>
        /// Use Profile API first, then Search API for additional context.
        /// </summary>
        ProfileFirst,

        /// <summary>
        /// Use Search API only.
        /// </summary>
        SearchOnly,

        /// <summary>
        /// Use Profile API only (static + dynamic memories).
        /// </summary>
        ProfileOnly,

        /// <summary>
        /// Use both Profile and Search APIs in parallel and merge results.
        /// </summary>
        Both
    }

}
