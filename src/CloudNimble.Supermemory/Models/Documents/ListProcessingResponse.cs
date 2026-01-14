using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents the response from listing documents currently being processed.
    /// </summary>
    public class ListProcessingResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of documents being processed.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<DocumentResponse> Documents { get; set; } = [];

        /// <summary>
        /// Gets or sets the total count of documents being processed.
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        #endregion

    }

}
