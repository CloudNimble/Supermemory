using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ConnectionResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ConnectionResponseTests
    {

        [TestMethod]
        [Description("GET /v3/connections/{connectionId} - 200 response Google Drive")]
        public void Deserialize_GoogleDrive_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_123",
                    "createdAt": "2024-01-10T08:00:00.000Z",
                    "provider": "google_drive",
                    "documentLimit": 1000,
                    "email": "user@example.com"
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResponse>(json, SupermemoryJsonContext.Default.ConnectionResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("conn_123");
            response.Provider.Should().Be(ConnectionProvider.GoogleDrive);
            response.DocumentLimit.Should().Be(1000);
            response.Email.Should().Be("user@example.com");
        }

        [TestMethod]
        [Description("GET /v3/connections/{connectionId} - 200 response Notion")]
        public void Deserialize_Notion_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_456",
                    "createdAt": "2024-01-15T10:30:00.000Z",
                    "provider": "notion",
                    "email": "workspace@notion.so"
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResponse>(json, SupermemoryJsonContext.Default.ConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.Notion);
        }

        [TestMethod]
        [Description("GET /v3/connections/{connectionId} - 200 response with expiration")]
        public void Deserialize_WithExpiration_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_789",
                    "createdAt": "2024-01-01T00:00:00.000Z",
                    "provider": "onedrive",
                    "expiresAt": "2024-02-01T00:00:00.000Z"
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResponse>(json, SupermemoryJsonContext.Default.ConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.OneDrive);
            response.ExpiresAt.Should().NotBeNull();
        }

        [TestMethod]
        [Description("GET /v3/connections/{connectionId} - 200 response with metadata")]
        public void Deserialize_WithMetadata_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_abc",
                    "createdAt": "2024-01-20T14:00:00.000Z",
                    "provider": "github",
                    "metadata": {
                        "org": "my-org",
                        "repos": 5
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResponse>(json, SupermemoryJsonContext.Default.ConnectionResponse);

            response.Should().NotBeNull();
            response!.Provider.Should().Be(ConnectionProvider.GitHub);
            response.Metadata.Should().NotBeNull();
        }

    }

}
