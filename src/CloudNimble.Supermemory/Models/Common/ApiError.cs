using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Common
{

    /// <summary>
    /// Represents an error response from the Supermemory API.
    /// </summary>
    public class ApiError
    {

        #region Properties

        /// <summary>
        /// Gets or sets the error type or category.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the detailed error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets additional error details.
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; set; }

        #endregion

    }

}
