using System.Net;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Exception thrown when a requested resource is not found.
    /// </summary>
    public class SupermemoryNotFoundException : SupermemoryApiException
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="apiError">The API error details.</param>
        public SupermemoryNotFoundException(string message, ApiError? apiError = null)
            : base(message, HttpStatusCode.NotFound, apiError)
        {
        }

        #endregion

    }

}
