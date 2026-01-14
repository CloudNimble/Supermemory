using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents a paginated list of documents.
    /// </summary>
    public class ListDocumentsResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of documents (called "memories" in the API).
        /// </summary>
        [JsonPropertyName("memories")]
        public List<DocumentResponse> Documents { get; set; } = [];

        /// <summary>
        /// Gets or sets the pagination metadata.
        /// </summary>
        [JsonPropertyName("pagination")]
        public PaginationInfo? Pagination { get; set; }

        #endregion

    }

}
