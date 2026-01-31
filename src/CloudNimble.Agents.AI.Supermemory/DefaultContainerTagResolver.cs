using System.Text.RegularExpressions;

namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Default implementation of <see cref="IContainerTagResolver"/> that supports template placeholder substitution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Supported placeholders:
    /// <list type="bullet">
    /// <item><description>{userId} - Replaced with <see cref="ContainerTagContext.UserId"/> or "anonymous"</description></item>
    /// <item><description>{sessionId} - Replaced with <see cref="ContainerTagContext.SessionId"/> or a new GUID</description></item>
    /// <item><description>{tenantId} - Replaced with <see cref="ContainerTagContext.TenantId"/> or "default"</description></item>
    /// <item><description>{threadId} - Replaced with <see cref="ContainerTagContext.ThreadId"/> or "main"</description></item>
    /// <item><description>{custom:key} - Replaced with the value from <see cref="ContainerTagContext.CustomValues"/></description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public partial class DefaultContainerTagResolver : IContainerTagResolver
    {

        #region Private Members

#if NET8_0
        private static readonly Regex CustomPlaceholderPattern = new(@"\{custom:(\w+)\}", RegexOptions.Compiled);
#endif

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public string? Resolve(string? template, ContainerTagContext context)
        {
            if (string.IsNullOrEmpty(template))
            {
                return null;
            }

            ArgumentNullException.ThrowIfNull(context);

            var result = template
                .Replace("{userId}", context.UserId ?? "anonymous")
                .Replace("{sessionId}", context.SessionId ?? Guid.NewGuid().ToString("N"))
                .Replace("{tenantId}", context.TenantId ?? "default")
                .Replace("{threadId}", context.ThreadId ?? "main");

            // Handle {custom:key} placeholders
            if (context.CustomValues != null && context.CustomValues.Count > 0)
            {
#if NET8_0
                result = CustomPlaceholderPattern.Replace(result, match =>
#else
                result = CustomPlaceholderRegex().Replace(result, match =>
#endif
                {
                    var key = match.Groups[1].Value;
                    if (context.CustomValues.TryGetValue(key, out var value))
                    {
                        return value?.ToString() ?? string.Empty;
                    }
                    return match.Value; // Keep original if key not found
                });
            }

            return result;
        }

        #endregion

        #region Source Generators

#if NET9_0_OR_GREATER
        [GeneratedRegex(@"\{custom:(\w+)\}")]
        private static partial Regex CustomPlaceholderRegex();
#endif

        #endregion

    }

}
