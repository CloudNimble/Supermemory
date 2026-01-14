using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Memories
{

    /// <summary>
    /// Represents the response from a forget memory operation.
    /// </summary>
    public class ForgetMemoryResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the forgotten memory.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the memory was successfully forgotten.
        /// </summary>
        [JsonPropertyName("forgotten")]
        public bool Forgotten { get; set; }

        #endregion

    }

}
