using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="DeleteConnectionResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class DeleteConnectionResponseTests
    {

        [TestMethod]
        [Description("DELETE /v3/connections/{connectionId} - 200 response")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_123",
                    "provider": "google_drive"
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteConnectionResponse>(json, SupermemoryJsonContext.Default.DeleteConnectionResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("conn_123");
            response.Provider.Should().Be(ConnectionProvider.GoogleDrive);
        }

        [TestMethod]
        [Description("DELETE /v3/connections/{provider} - Notion provider")]
        public void Deserialize_NotionProvider_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_456",
                    "provider": "notion"
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteConnectionResponse>(json, SupermemoryJsonContext.Default.DeleteConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.Notion);
        }

        [TestMethod]
        [Description("DELETE /v3/connections/{connectionId} - OneDrive provider")]
        public void Deserialize_OneDriveProvider_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_789",
                    "provider": "onedrive"
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteConnectionResponse>(json, SupermemoryJsonContext.Default.DeleteConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.OneDrive);
        }

        [TestMethod]
        [Description("DELETE /v3/connections/{connectionId} - GitHub provider")]
        public void Deserialize_GithubProvider_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_abc",
                    "provider": "github"
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteConnectionResponse>(json, SupermemoryJsonContext.Default.DeleteConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.GitHub);
        }

    }

}
