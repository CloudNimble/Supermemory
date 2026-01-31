namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Defines the contract for resolving container tag templates to actual values.
    /// </summary>
    public interface IContainerTagResolver
    {

        /// <summary>
        /// Resolves a container tag template to an actual value.
        /// </summary>
        /// <param name="template">The template string containing placeholders (e.g., "user-{userId}-{sessionId}").</param>
        /// <param name="context">The context containing values to substitute for placeholders.</param>
        /// <returns>The resolved container tag, or null if the template is null or empty.</returns>
        string? Resolve(string? template, ContainerTagContext context);

    }

}
