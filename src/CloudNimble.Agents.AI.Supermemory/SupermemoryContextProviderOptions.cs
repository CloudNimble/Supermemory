using CloudNimble.Supermemory;
using Microsoft.Extensions.Configuration;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Configuration options for the <see cref="SupermemoryContextProvider"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is designed to be used with the .NET Options pattern for configuration binding.
    /// Configuration can come from any .NET configuration source.
    /// </para>
    /// <para>
    /// Example configuration in <c>appsettings.json</c>:
    /// <code>
    /// {
    ///   "SupermemoryContext": {
    ///     "DefaultContainerTag": "user-{sessionId}",
    ///     "SearchLimit": 5,
    ///     "MinimumSimilarityScore": 0.7,
    ///     "UseProfileApi": true,
    ///     "UseSearchApi": true
    ///   }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public class SupermemoryContextProviderOptions
    {

        #region Constants

        /// <summary>
        /// The default configuration section name for <see cref="SupermemoryContextProviderOptions"/>.
        /// </summary>
        public const string SectionName = "SupermemoryContext";

        #endregion

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

        #region Public Methods

        /// <summary>
        /// Loads configuration values from the specified section into this options instance.
        /// This method is AOT-compatible and does not use reflection.
        /// </summary>
        /// <param name="section">The configuration section to load from.</param>
        public void LoadFrom(IConfigurationSection section)
        {
            ArgumentNullException.ThrowIfNull(section);

            var defaultContainerTag = section[nameof(DefaultContainerTag)];
            if (!string.IsNullOrWhiteSpace(defaultContainerTag))
            {
                DefaultContainerTag = defaultContainerTag;
            }

            var retrievalStrategyStr = section[nameof(RetrievalStrategy)];
            if (!string.IsNullOrWhiteSpace(retrievalStrategyStr) && Enum.TryParse<MemoryRetrievalStrategy>(retrievalStrategyStr, out var retrievalStrategy))
            {
                RetrievalStrategy = retrievalStrategy;
            }

            var useProfileApiStr = section[nameof(UseProfileApi)];
            if (!string.IsNullOrWhiteSpace(useProfileApiStr) && bool.TryParse(useProfileApiStr, out var useProfileApi))
            {
                UseProfileApi = useProfileApi;
            }

            var useSearchApiStr = section[nameof(UseSearchApi)];
            if (!string.IsNullOrWhiteSpace(useSearchApiStr) && bool.TryParse(useSearchApiStr, out var useSearchApi))
            {
                UseSearchApi = useSearchApi;
            }

            var searchModeStr = section[nameof(SearchMode)];
            if (!string.IsNullOrWhiteSpace(searchModeStr) && Enum.TryParse<SearchMode>(searchModeStr, out var searchMode))
            {
                SearchMode = searchMode;
            }

            var searchLimitStr = section[nameof(SearchLimit)];
            if (!string.IsNullOrWhiteSpace(searchLimitStr) && int.TryParse(searchLimitStr, out var searchLimit))
            {
                SearchLimit = searchLimit;
            }

            var minimumSimilarityScoreStr = section[nameof(MinimumSimilarityScore)];
            if (!string.IsNullOrWhiteSpace(minimumSimilarityScoreStr) && double.TryParse(minimumSimilarityScoreStr, out var minimumSimilarityScore))
            {
                MinimumSimilarityScore = minimumSimilarityScore;
            }

            var rerankResultsStr = section[nameof(RerankResults)];
            if (!string.IsNullOrWhiteSpace(rerankResultsStr) && bool.TryParse(rerankResultsStr, out var rerankResults))
            {
                RerankResults = rerankResults;
            }

            var profileThresholdStr = section[nameof(ProfileThreshold)];
            if (!string.IsNullOrWhiteSpace(profileThresholdStr) && double.TryParse(profileThresholdStr, out var profileThreshold))
            {
                ProfileThreshold = profileThreshold;
            }

            var enableConversationStorageStr = section[nameof(EnableConversationStorage)];
            if (!string.IsNullOrWhiteSpace(enableConversationStorageStr) && bool.TryParse(enableConversationStorageStr, out var enableConversationStorage))
            {
                EnableConversationStorage = enableConversationStorage;
            }

            var storageFormatStr = section[nameof(StorageFormat)];
            if (!string.IsNullOrWhiteSpace(storageFormatStr) && Enum.TryParse<ConversationStorageFormat>(storageFormatStr, out var storageFormat))
            {
                StorageFormat = storageFormat;
            }

            var storeUserMessagesOnlyStr = section[nameof(StoreUserMessagesOnly)];
            if (!string.IsNullOrWhiteSpace(storeUserMessagesOnlyStr) && bool.TryParse(storeUserMessagesOnlyStr, out var storeUserMessagesOnly))
            {
                StoreUserMessagesOnly = storeUserMessagesOnly;
            }

            var instructionTemplate = section[nameof(InstructionTemplate)];
            if (!string.IsNullOrWhiteSpace(instructionTemplate))
            {
                InstructionTemplate = instructionTemplate;
            }
        }

        #endregion

    }

}
