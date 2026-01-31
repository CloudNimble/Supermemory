namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Specifies the format for storing conversations in Supermemory.
    /// </summary>
    public enum ConversationStorageFormat
    {
        /// <summary>
        /// Store as markdown-formatted conversation.
        /// </summary>
        Markdown,

        /// <summary>
        /// Store as JSON array of messages.
        /// </summary>
        Json,

        /// <summary>
        /// Store as plain text with role prefixes.
        /// </summary>
        PlainText
    }

}
