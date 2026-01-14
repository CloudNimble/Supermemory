using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Represents the processing status of a document in Supermemory.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<DocumentStatus>))]
    public enum DocumentStatus
    {

        /// <summary>
        /// The document status is unknown.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("unknown")]
#endif
        Unknown,

        /// <summary>
        /// The document is queued for processing.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("queued")]
#endif
        Queued,

        /// <summary>
        /// The document content is being extracted.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("extracting")]
#endif
        Extracting,

        /// <summary>
        /// The document is being chunked into smaller pieces.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("chunking")]
#endif
        Chunking,

        /// <summary>
        /// Embeddings are being generated for the document.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("embedding")]
#endif
        Embedding,

        /// <summary>
        /// The document is being indexed.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("indexing")]
#endif
        Indexing,

        /// <summary>
        /// The document has been fully processed.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("done")]
#endif
        Done,

        /// <summary>
        /// The document processing failed.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("failed")]
#endif
        Failed

    }

}
