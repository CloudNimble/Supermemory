using System.Text.Json.Serialization;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Provides context values for resolving container tag templates.
    /// </summary>
    public class ContainerTagContext
    {

        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        [JsonPropertyName("tenantId")]
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the thread identifier.
        /// </summary>
        [JsonPropertyName("threadId")]
        public string? ThreadId { get; set; }

        /// <summary>
        /// Gets or sets custom values for template placeholders.
        /// Use the format {custom:key} in templates to reference these values.
        /// </summary>
        [JsonPropertyName("customValues")]
        public Dictionary<string, object>? CustomValues { get; set; }

        #endregion

    }

}
