using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory.Models.Documents
{

    /// <summary>
    /// Represents options for uploading a file.
    /// </summary>
    public class UploadFileRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the container tags for the uploaded document.
        /// </summary>
        [JsonPropertyName("containerTags")]
        public List<string>? ContainerTags { get; set; }

        /// <summary>
        /// Gets or sets the file type hint.
        /// </summary>
        [JsonPropertyName("fileType")]
        public string? FileType { get; set; }

        /// <summary>
        /// Gets or sets the metadata for the document.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the file.
        /// </summary>
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        /// <summary>
        /// Gets or sets whether to use advanced processing for the file.
        /// </summary>
        [JsonPropertyName("useAdvancedProcessing")]
        public bool? UseAdvancedProcessing { get; set; }

        #endregion

    }

}
