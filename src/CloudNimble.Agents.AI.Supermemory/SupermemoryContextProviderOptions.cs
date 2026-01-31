using CloudNimble.Supermemory;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryContextProvider"/>.
    /// </summary>
    public class SupermemoryContextProviderOptions
    {

        #region Properties

        /// <summary>
        /// Gets or sets the default container tag. Can be overridden per-session via the ContainerTag property.
        /// Supports template placeholders: {userId}, {sessionId}, {tenantId}, {threadId}.
        /// </summary>
        public string? DefaultContainerTag { get; set; }

        /// <summary>
        /// Gets or sets the strategy for retrieving memories. Default: <see cref="MemoryRetrievalStrategy.ProfileFirst"/>.
        /// </summary>
        public MemoryRetrievalStrategy RetrievalStrategy { get; set; } = MemoryRetrievalStrategy.ProfileFirst;

        /// <summary>
        /// Gets or sets whether to use the Profile API for static/dynamic memories. Default: true.
        /// </summary>
        public bool UseProfileApi { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use the Search API for query-based memory retrieval. Default: true.
        /// </summary>
        public bool UseSearchApi { get; set; } = true;

        /// <summary>
        /// Gets or sets the search mode when using the Search API. Default: Memories.
        /// </summary>
        public SearchMode SearchMode { get; set; } = SearchMode.Memories;

        /// <summary>
        /// Gets or sets the maximum number of memories to retrieve from search. Default: 5.
        /// </summary>
        public int SearchLimit { get; set; } = 5;

        /// <summary>
        /// Gets or sets the minimum similarity score for search results. Default: 0.7.
        /// </summary>
        public double MinimumSimilarityScore { get; set; } = 0.7;

        /// <summary>
        /// Gets or sets whether to rerank search results. Default: false.
        /// </summary>
        public bool RerankResults { get; set; }

        /// <summary>
        /// Gets or sets the profile score threshold. Default: 0.7.
        /// </summary>
        public double ProfileThreshold { get; set; } = 0.7;

        /// <summary>
        /// Gets or sets whether to store conversations to Supermemory for automatic memory extraction. Default: true.
        /// Supermemory automatically extracts facts and updates user profiles from conversation content.
        /// </summary>
        public bool EnableConversationStorage { get; set; } = true;

        /// <summary>
        /// Gets or sets the format for storing conversations. Default: <see cref="ConversationStorageFormat.Markdown"/>.
        /// </summary>
        public ConversationStorageFormat StorageFormat { get; set; } = ConversationStorageFormat.Markdown;

        /// <summary>
        /// Gets or sets whether to store only user messages (excluding assistant responses). Default: false.
        /// When false, stores the full conversation for richer context extraction.
        /// </summary>
        public bool StoreUserMessagesOnly { get; set; }

        /// <summary>
        /// Gets or sets the template for formatting memories as context instructions.
        /// Placeholders: {staticMemories}, {dynamicMemories}, {searchMemories}.
        /// </summary>
        public string InstructionTemplate { get; set; } = """
            ## User Context

            ### Known Facts (Static)
            {staticMemories}

            ### Recent Context (Dynamic)
            {dynamicMemories}

            ### Relevant Memories
            {searchMemories}

            Use this context to personalize your responses.
            """;

        /// <summary>
        /// Gets or sets default metadata to include with stored memories.
        /// </summary>
        public Dictionary<string, object>? DefaultMetadata { get; set; }

        #endregion

    }

}
