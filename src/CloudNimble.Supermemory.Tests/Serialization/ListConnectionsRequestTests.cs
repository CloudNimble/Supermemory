using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ListConnectionsRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ListConnectionsRequestTests
    {

        [TestMethod]
        [Description("POST /v3/connections/list - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new ListConnectionsRequest
            {
                ContainerTags = ["user_123", "team_456"]
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ListConnectionsRequest);

            json.Should().Contain("\"containerTags\":");
            json.Should().Contain("\"user_123\"");
            json.Should().Contain("\"team_456\"");
        }

        [TestMethod]
        [Description("POST /v3/connections/list - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTags": ["user_789"]
                }
                """;

            var request = JsonSerializer.Deserialize<ListConnectionsRequest>(json, SupermemoryJsonContext.Default.ListConnectionsRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().HaveCount(1);
            request.ContainerTags.Should().Contain("user_789");
        }

        [TestMethod]
        [Description("POST /v3/connections/list - Empty request")]
        public void Deserialize_EmptyRequest_ReturnsValidObject()
        {
            var json = """
                {}
                """;

            var request = JsonSerializer.Deserialize<ListConnectionsRequest>(json, SupermemoryJsonContext.Default.ListConnectionsRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().BeNull();
        }

    }

}
