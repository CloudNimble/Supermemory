using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Common
{

    /// <summary>
    /// Represents a single filter condition.
    /// </summary>
    public class FilterCondition
    {

        #region Properties

        /// <summary>
        /// Gets or sets the field name to filter on.
        /// </summary>
        [JsonPropertyName("field")]
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the filter operator.
        /// </summary>
        [JsonPropertyName("operator")]
        public string Operator { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value to compare against.
        /// </summary>
        [JsonPropertyName("value")]
        public object? Value { get; set; }

        #endregion

    }

}
