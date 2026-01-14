using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ConnectionResourcesResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ConnectionResourcesResponseTests
    {

        [TestMethod]
        [Description("GET /v3/connections/{connectionId}/resources - 200 response")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "resources": [
                        {
                            "id": "folder_123",
                            "name": "Documents",
                            "type": "folder"
                        },
                        {
                            "id": "folder_456",
                            "name": "Projects",
                            "type": "folder"
                        }
                    ],
                    "total_count": 2
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResourcesResponse>(json, SupermemoryJsonContext.Default.ConnectionResourcesResponse);

            response.Should().NotBeNull();
            response!.Resources.Should().HaveCount(2);
            response.TotalCount.Should().Be(2);
            response.Resources[0].Id.Should().Be("folder_123");
            response.Resources[0].Name.Should().Be("Documents");
        }

        [TestMethod]
        [Description("GET /v3/connections/{connectionId}/resources - Empty response")]
        public void Deserialize_EmptyResponse_ReturnsValidObject()
        {
            var json = """
                {
                    "resources": [],
                    "total_count": 0
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResourcesResponse>(json, SupermemoryJsonContext.Default.ConnectionResourcesResponse);

            response.Should().NotBeNull();
            response!.Resources.Should().BeEmpty();
            response.TotalCount.Should().Be(0);
        }

        [TestMethod]
        [Description("GET /v3/connections/{connectionId}/resources - Various resource types")]
        public void Deserialize_VariousTypes_ReturnsValidObject()
        {
            var json = """
                {
                    "resources": [
                        {
                            "id": "db_001",
                            "name": "Tasks Database",
                            "type": "database"
                        },
                        {
                            "id": "page_002",
                            "name": "Meeting Notes",
                            "type": "page"
                        },
                        {
                            "id": "file_003",
                            "name": "Report.pdf",
                            "type": "file"
                        }
                    ],
                    "total_count": 3
                }
                """;

            var response = JsonSerializer.Deserialize<ConnectionResourcesResponse>(json, SupermemoryJsonContext.Default.ConnectionResourcesResponse);

            response.Should().NotBeNull();
            response!.Resources.Should().HaveCount(3);
            response.Resources[0].Type.Should().Be("database");
            response.Resources[1].Type.Should().Be("page");
            response.Resources[2].Type.Should().Be("file");
        }

    }

}
