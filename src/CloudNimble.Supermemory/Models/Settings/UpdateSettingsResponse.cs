using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Settings
{

    /// <summary>
    /// Represents the response from updating organization settings.
    /// </summary>
    public class UpdateSettingsResponse
    {

        #region Properties

        /// <summary>
        /// Gets or sets the organization ID.
        /// </summary>
        [JsonPropertyName("orgId")]
        public string OrgId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the organization slug.
        /// </summary>
        [JsonPropertyName("orgSlug")]
        public string OrgSlug { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated settings.
        /// </summary>
        [JsonPropertyName("updated")]
        public SettingsResponse? Updated { get; set; }

        #endregion

    }

}
