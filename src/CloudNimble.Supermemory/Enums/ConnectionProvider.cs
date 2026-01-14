using System.Text.Json.Serialization;

namespace CloudNimble.Supermemory
{

    /// <summary>
    /// Represents the available connection providers in Supermemory.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ConnectionProvider>))]
    public enum ConnectionProvider
    {

        /// <summary>
        /// Notion workspace connection.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("notion")]
#endif
        Notion,

        /// <summary>
        /// Google Drive connection.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("google_drive")]
#endif
        GoogleDrive,

        /// <summary>
        /// Microsoft OneDrive connection.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("onedrive")]
#endif
        OneDrive,

        /// <summary>
        /// GitHub repository connection.
        /// </summary>
#if NET9_0_OR_GREATER
        [JsonStringEnumMemberName("github")]
#endif
        GitHub

    }

}
