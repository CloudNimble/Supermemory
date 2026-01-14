using System.Net;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Exception thrown when the API returns a validation error (400 Bad Request).
    /// </summary>
    public class SupermemoryValidationException : SupermemoryApiException
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="apiError">The API error details.</param>
        public SupermemoryValidationException(string message, ApiError? apiError = null)
            : base(message, HttpStatusCode.BadRequest, apiError)
        {
        }

        #endregion

    }

}
