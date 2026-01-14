using System.Net.Http.Headers;
using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Documents;

namespace CloudNimble.Supermemory.Resources
{

    /// <summary>
    /// Provides access to document management operations in the Supermemory API.
    /// </summary>
    public class DocumentsResource : ResourceBase
    {

        #region Constants

        private const string BasePath = "/v3/documents";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsResource"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="options">The client options.</param>
        internal DocumentsResource(HttpClient httpClient, SupermemoryClientOptions options)
            : base(httpClient, options)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new document to Supermemory.
        /// </summary>
        /// <param name="request">The add document request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the document ID and status.</returns>
        /// <remarks>
        /// <para>
        /// The content can be a URL, raw text, or structured data.
        /// Documents are processed asynchronously after being added.
        /// </para>
        /// </remarks>
        public Task<DocumentResponse> AddAsync(
            AddDocumentRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                BasePath,
                request,
                SupermemoryJsonContext.Default.AddDocumentRequest,
                SupermemoryJsonContext.Default.DocumentResponse,
                cancellationToken);
        }

        /// <summary>
        /// Adds multiple documents to Supermemory in a single request.
        /// </summary>
        /// <param name="request">The batch add documents request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the results for each document.</returns>
        public Task<BatchAddDocumentsResponse> BatchAddAsync(
            BatchAddDocumentsRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return PostAsync(
                $"{BasePath}/batch",
                request,
                SupermemoryJsonContext.Default.BatchAddDocumentsRequest,
                SupermemoryJsonContext.Default.BatchAddDocumentsResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets a document by its ID.
        /// </summary>
        /// <param name="documentId">The document ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The document details.</returns>
        public Task<DocumentResponse> GetAsync(
            string documentId,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
            return GetAsync(
                $"{BasePath}/{Uri.EscapeDataString(documentId)}",
                SupermemoryJsonContext.Default.DocumentResponse,
                cancellationToken);
        }

        /// <summary>
        /// Updates an existing document.
        /// </summary>
        /// <param name="documentId">The document ID.</param>
        /// <param name="request">The update document request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated document details.</returns>
        public Task<DocumentResponse> UpdateAsync(
            string documentId,
            UpdateDocumentRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
            ArgumentNullException.ThrowIfNull(request);
            return PatchAsync(
                $"{BasePath}/{Uri.EscapeDataString(documentId)}",
                request,
                SupermemoryJsonContext.Default.UpdateDocumentRequest,
                SupermemoryJsonContext.Default.DocumentResponse,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a document by its ID.
        /// </summary>
        /// <param name="documentId">The document ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A status response indicating success.</returns>
        public Task<StatusResponse> DeleteAsync(
            string documentId,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
            return DeleteAsync(
                $"{BasePath}/{Uri.EscapeDataString(documentId)}",
                SupermemoryJsonContext.Default.StatusResponse,
                cancellationToken);
        }

        /// <summary>
        /// Deletes multiple documents in bulk.
        /// </summary>
        /// <param name="request">The bulk delete request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing deletion results.</returns>
        public Task<DeleteBulkResponse> DeleteBulkAsync(
            DeleteBulkRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return DeleteAsync(
                $"{BasePath}/bulk",
                request,
                SupermemoryJsonContext.Default.DeleteBulkRequest,
                SupermemoryJsonContext.Default.DeleteBulkResponse,
                cancellationToken);
        }

        /// <summary>
        /// Lists documents with optional filtering.
        /// </summary>
        /// <param name="request">The list documents request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The paginated list of documents.</returns>
        public Task<ListDocumentsResponse> ListAsync(
            ListDocumentsRequest? request = null,
            CancellationToken cancellationToken = default)
        {
            var body = request ?? new ListDocumentsRequest();
            return PostAsync(
                $"{BasePath}/list",
                body,
                SupermemoryJsonContext.Default.ListDocumentsRequest,
                SupermemoryJsonContext.Default.ListDocumentsResponse,
                cancellationToken);
        }

        /// <summary>
        /// Gets a list of documents that are currently being processed.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of processing documents.</returns>
        public Task<ListProcessingResponse> ListProcessingAsync(
            CancellationToken cancellationToken = default)
        {
            return GetAsync(
                $"{BasePath}/processing",
                SupermemoryJsonContext.Default.ListProcessingResponse,
                cancellationToken);
        }

        /// <summary>
        /// Uploads a file to Supermemory.
        /// </summary>
        /// <param name="fileStream">The file stream to upload.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="request">Optional upload file request with additional metadata.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the document ID.</returns>
        public async Task<DocumentResponse> UploadFileAsync(
            Stream fileStream,
            string fileName,
            UploadFileRequest? request = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(fileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(fileName));
            content.Add(fileContent, "file", fileName);

            if (request?.ContainerTags?.Count > 0)
            {
                foreach (var tag in request.ContainerTags)
                {
                    content.Add(new StringContent(tag), "containerTags");
                }
            }

            if (request?.Metadata != null)
            {
                foreach (var kvp in request.Metadata)
                {
                    content.Add(new StringContent(kvp.Value?.ToString() ?? string.Empty), $"metadata[{kvp.Key}]");
                }
            }

            return await PostMultipartAsync(
                $"{BasePath}/file",
                content,
                SupermemoryJsonContext.Default.DocumentResponse,
                cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the MIME type for a file based on its extension.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The MIME type.</returns>
        private static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".md" => "text/markdown",
                ".html" => "text/html",
                ".htm" => "text/html",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".csv" => "text/csv",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        #endregion

    }

}
