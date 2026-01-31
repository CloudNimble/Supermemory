using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Common
{

    /// <summary>
    /// Represents a filter expression for querying documents.
    /// </summary>
    public class FilterExpression
    {

        #region Properties

        /// <summary>
        /// Gets or sets the OR conditions.
        /// </summary>
        [JsonPropertyName("or")]
        public List<FilterCondition>? Or { get; set; }

        /// <summary>
        /// Gets or sets the AND conditions.
        /// </summary>
        [JsonPropertyName("and")]
        public List<FilterCondition>? And { get; set; }

        #endregion

    }

}
