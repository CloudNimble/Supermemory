namespace CloudNimble.Agents.AI.Supermemory
{

    /// <summary>
    /// Specifies when to trigger the chat reducer.
    /// </summary>
    public enum ChatReducerTriggerEvent
    {
        /// <summary>
        /// Apply reducer after a message is added.
        /// </summary>
        AfterMessageAdded,

        /// <summary>
        /// Apply reducer before messages are retrieved.
        /// </summary>
        BeforeMessagesRetrieval
    }

}
