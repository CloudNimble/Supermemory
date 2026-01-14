namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Base exception for all Supermemory client errors.
    /// </summary>
    public class SupermemoryException : Exception
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryException"/> class.
        /// </summary>
        public SupermemoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public SupermemoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SupermemoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

    }

}
