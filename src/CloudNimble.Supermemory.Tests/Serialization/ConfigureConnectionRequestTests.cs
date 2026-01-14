using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ConfigureConnectionRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ConfigureConnectionRequestTests
    {

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new ConfigureConnectionRequest
            {
                Resources =
                [
                    new ConnectionResource
                    {
                        Id = "folder_123",
                        Name = "Documents",
                        Type = "folder"
                    },
                    new ConnectionResource
                    {
                        Id = "folder_456",
                        Name = "Projects",
                        Type = "folder"
                    }
                ]
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ConfigureConnectionRequest);

            json.Should().Contain("\"resources\":");
            json.Should().Contain("\"id\":\"folder_123\"");
            json.Should().Contain("\"name\":\"Documents\"");
            json.Should().Contain("\"type\":\"folder\"");
        }

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "resources": [
                        {
                            "id": "page_abc",
                            "name": "Meeting Notes",
                            "type": "page"
                        }
                    ]
                }
                """;

            var request = JsonSerializer.Deserialize<ConfigureConnectionRequest>(json, SupermemoryJsonContext.Default.ConfigureConnectionRequest);

            request.Should().NotBeNull();
            request!.Resources.Should().HaveCount(1);
            request.Resources[0].Id.Should().Be("page_abc");
            request.Resources[0].Name.Should().Be("Meeting Notes");
            request.Resources[0].Type.Should().Be("page");
        }

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - Empty resources")]
        public void Deserialize_EmptyResources_ReturnsValidObject()
        {
            var json = """
                {
                    "resources": []
                }
                """;

            var request = JsonSerializer.Deserialize<ConfigureConnectionRequest>(json, SupermemoryJsonContext.Default.ConfigureConnectionRequest);

            request.Should().NotBeNull();
            request!.Resources.Should().BeEmpty();
        }

    }

}
