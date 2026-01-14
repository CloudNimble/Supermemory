using System.Text.Json.Serialization;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents the response from a batch add documents operation.
    /// </summary>
    public class BatchAddDocumentsResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of status responses for each document.
        /// </summary>
        [JsonPropertyName("results")]
        public List<StatusResponse> Results { get; set; } = [];

        #endregion

    }

}
