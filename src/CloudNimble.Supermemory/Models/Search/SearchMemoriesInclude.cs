using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Search
{

    /// <summary>
    /// Specifies what to include in the search memories response.
    /// </summary>
    public class SearchMemoriesInclude
    {

        #region Properties

        /// <summary>
        /// Gets or sets whether to include chunks.
        /// </summary>
        [JsonPropertyName("chunks")]
        public bool? Chunks { get; set; }

        /// <summary>
        /// Gets or sets whether to include documents.
        /// </summary>
        [JsonPropertyName("documents")]
        public bool? Documents { get; set; }

        /// <summary>
        /// Gets or sets whether to include forgotten memories.
        /// </summary>
        [JsonPropertyName("forgottenMemories")]
        public bool? ForgottenMemories { get; set; }

        /// <summary>
        /// Gets or sets whether to include related memories.
        /// </summary>
        [JsonPropertyName("relatedMemories")]
        public bool? RelatedMemories { get; set; }

        /// <summary>
        /// Gets or sets whether to include summaries.
        /// </summary>
        [JsonPropertyName("summaries")]
        public bool? Summaries { get; set; }

        #endregion

    }

}
