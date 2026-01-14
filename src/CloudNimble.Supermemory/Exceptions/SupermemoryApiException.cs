using System.Net;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Exceptions
{

    /// <summary>
    /// Exception thrown when the Supermemory API returns an error response.
    /// </summary>
    public class SupermemoryApiException : SupermemoryException
    {

        #region Properties

        /// <summary>
        /// Gets the HTTP status code from the API response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the API error details, if available.
        /// </summary>
        public ApiError? ApiError { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="apiError">The API error details.</param>
        public SupermemoryApiException(string message, HttpStatusCode statusCode, ApiError? apiError = null)
            : base(message)
        {
            StatusCode = statusCode;
            ApiError = apiError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupermemoryApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="innerException">The inner exception.</param>
        public SupermemoryApiException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        #endregion

    }

}
