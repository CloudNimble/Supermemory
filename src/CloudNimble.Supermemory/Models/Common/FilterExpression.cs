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
