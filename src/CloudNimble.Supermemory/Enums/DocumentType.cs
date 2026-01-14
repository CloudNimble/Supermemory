using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Represents the type of content in a Supermemory document.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<DocumentType>))]
    public enum DocumentType
    {

        /// <summary>
        /// Plain text content.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("text")]
#endif
        Text,

        /// <summary>
        /// PDF document.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("pdf")]
#endif
        Pdf,

        /// <summary>
        /// Tweet content.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("tweet")]
#endif
        Tweet,

        /// <summary>
        /// Google Docs document.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("google_doc")]
#endif
        GoogleDoc,

        /// <summary>
        /// Google Slides presentation.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("google_slide")]
#endif
        GoogleSlide,

        /// <summary>
        /// Google Sheets spreadsheet.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("google_sheet")]
#endif
        GoogleSheet,

        /// <summary>
        /// Image file.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("image")]
#endif
        Image,

        /// <summary>
        /// Video file.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("video")]
#endif
        Video,

        /// <summary>
        /// Notion document.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("notion_doc")]
#endif
        NotionDoc,

        /// <summary>
        /// Web page content.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("webpage")]
#endif
        Webpage,

        /// <summary>
        /// OneDrive document.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("onedrive")]
#endif
        OneDrive,

        /// <summary>
        /// GitHub markdown file.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("github_markdown")]
#endif
        GitHubMarkdown

    }

}
