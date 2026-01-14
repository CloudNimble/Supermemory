using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Base class for all Supermemory API resources.
    /// Provides common HTTP request handling and JSON serialization support.
    /// </summary>
    public abstract class ResourceBase
    {

        #region Private Members

        private readonly HttpClient _httpClient;
        private readonly SupermemoryClientOptions _options;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBase"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        protected ResourceBase(HttpClient httpClient, SupermemoryClientOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sends a GET request to the specified path.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> GetAsync<TResponse>(
            string path,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Get, path);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a POST request to the specified path with a JSON body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="requestTypeInfo">The JSON type info for the request.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest body,
            JsonTypeInfo<TRequest> requestTypeInfo,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Post, path);
            request.Content = CreateJsonContent(body, requestTypeInfo);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a POST request to the specified path without a body.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> PostAsync<TResponse>(
            string path,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Post, path);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a PATCH request to the specified path with a JSON body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="requestTypeInfo">The JSON type info for the request.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> PatchAsync<TRequest, TResponse>(
            string path,
            TRequest body,
            JsonTypeInfo<TRequest> requestTypeInfo,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Patch, path);
            request.Content = CreateJsonContent(body, requestTypeInfo);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a DELETE request to the specified path.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> DeleteAsync<TResponse>(
            string path,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Delete, path);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a DELETE request to the specified path with a JSON body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="requestTypeInfo">The JSON type info for the request.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> DeleteAsync<TRequest, TResponse>(
            string path,
            TRequest body,
            JsonTypeInfo<TRequest> requestTypeInfo,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Delete, path);
            request.Content = CreateJsonContent(body, requestTypeInfo);
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a multipart form data POST request to the specified path.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="content">The multipart form data content.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        protected async Task<TResponse> PostMultipartAsync<TResponse>(
            string path,
            MultipartFormDataContent content,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateRequest(HttpMethod.Post, path);
            request.Content = content;
            return await SendAsync(request, responseTypeInfo, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates an HTTP request message with the appropriate headers.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The API path.</param>
        /// <returns>The HTTP request message.</returns>
        private HttpRequestMessage CreateRequest(HttpMethod method, string path)
        {
            var baseUrl = _options.BaseUrl.TrimEnd('/');
            var requestUri = new Uri($"{baseUrl}{path}");

            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return request;
        }

        /// <summary>
        /// Creates JSON content from an object using source-generated serialization.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <param name="typeInfo">The JSON type info for the object.</param>
        /// <returns>The JSON content.</returns>
        private static StringContent CreateJsonContent<T>(T value, JsonTypeInfo<T> typeInfo)
        {
            var json = JsonSerializer.Serialize(value, typeInfo);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Sends an HTTP request and deserializes the response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The HTTP request.</param>
        /// <param name="responseTypeInfo">The JSON type info for the response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        private async Task<TResponse> SendAsync<TResponse>(
            HttpRequestMessage request,
            JsonTypeInfo<TResponse> responseTypeInfo,
            CancellationToken cancellationToken)
        {
            var retryCount = 0;
            HttpResponseMessage? response = null;

            while (retryCount <= _options.MaxRetries)
            {
                try
                {
                    response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                        var result = JsonSerializer.Deserialize(content, responseTypeInfo);
                        return result ?? throw new SupermemoryException("Failed to deserialize response.");
                    }

                    // Handle specific error cases
                    await HandleErrorResponseAsync(response, cancellationToken).ConfigureAwait(false);
                }
                catch (HttpRequestException) when (retryCount < _options.MaxRetries)
                {
                    retryCount++;
                    await Task.Delay(GetRetryDelay(retryCount), cancellationToken).ConfigureAwait(false);

                    // Clone the request for retry since HttpRequestMessage can only be sent once
                    request = await CloneRequestAsync(request, cancellationToken).ConfigureAwait(false);
                    continue;
                }
                catch (SupermemoryException)
                {
                    throw;
                }

                break;
            }

            // This should not be reached, but just in case
            throw new SupermemoryException("Request failed after maximum retries.");
        }

        /// <summary>
        /// Handles error responses by throwing appropriate exceptions.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private static async Task HandleErrorResponseAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            ApiError? apiError = null;

            try
            {
                apiError = JsonSerializer.Deserialize(content, SupermemoryJsonContext.Default.ApiError);
            }
            catch
            {
                // If we can't deserialize the error, create a generic one
            }

            var errorMessage = apiError?.Error ?? apiError?.Message ?? content;

            throw response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new SupermemoryAuthenticationException(errorMessage, apiError),
                HttpStatusCode.NotFound => new SupermemoryNotFoundException(errorMessage, apiError),
                HttpStatusCode.TooManyRequests => new SupermemoryRateLimitException(errorMessage, apiError),
                HttpStatusCode.BadRequest => new SupermemoryValidationException(errorMessage, apiError),
                _ => new SupermemoryApiException(errorMessage, response.StatusCode, apiError)
            };
        }

        /// <summary>
        /// Gets the delay before the next retry attempt.
        /// </summary>
        /// <param name="retryCount">The current retry count.</param>
        /// <returns>The delay timespan.</returns>
        private static TimeSpan GetRetryDelay(int retryCount)
        {
            // Exponential backoff: 1s, 2s, 4s, etc.
            return TimeSpan.FromSeconds(Math.Pow(2, retryCount - 1));
        }

        /// <summary>
        /// Clones an HTTP request message for retry.
        /// </summary>
        /// <param name="request">The original request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A clone of the request.</returns>
        private static async Task<HttpRequestMessage> CloneRequestAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                clone.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            return clone;
        }

        #endregion

    }

}
