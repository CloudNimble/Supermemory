using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Represents the search mode for memory queries.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SearchMode>))]
    public enum SearchMode
    {

        /// <summary>
        /// Search through memories only.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("memories")]
#endif
        Memories,

        /// <summary>
        /// Hybrid search combining memories and document chunks.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("hybrid")]
#endif
        Hybrid

    }

}
