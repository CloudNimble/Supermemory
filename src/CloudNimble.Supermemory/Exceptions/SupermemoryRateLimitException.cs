using System.Net;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Exception thrown when the API rate limit is exceeded.
    /// </summary>
    public class SupermemoryRateLimitException : SupermemoryApiException
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryRateLimitException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="apiError">The API error details.</param>
        public SupermemoryRateLimitException(string message, ApiError? apiError = null)
            : base(message, HttpStatusCode.TooManyRequests, apiError)
        {
        }

        #endregion

    }

}
