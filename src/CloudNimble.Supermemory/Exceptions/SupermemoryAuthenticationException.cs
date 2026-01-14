using System.Net;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Exception thrown when authentication with the Supermemory API fails.
    /// This typically indicates an invalid or missing API key.
    /// </summary>
    public class SupermemoryAuthenticationException : SupermemoryApiException
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="apiError">The API error details.</param>
        public SupermemoryAuthenticationException(string message, ApiError? apiError = null)
            : base(message, HttpStatusCode.Unauthorized, apiError)
        {
        }

        #endregion

    }

}
