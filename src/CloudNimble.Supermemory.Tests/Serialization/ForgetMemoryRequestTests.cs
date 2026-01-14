using System.Text.Json;
using CloudNimble.Supermemory.Models.Memories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ForgetMemoryRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ForgetMemoryRequestTests
    {

        [TestMethod]
        [Description("DELETE /v4/memories - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new ForgetMemoryRequest
            {
                ContainerTag = "user_123",
                Id = "mem_abc123",
                Reason = "User requested deletion"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ForgetMemoryRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"id\":\"mem_abc123\"");
            json.Should().Contain("\"reason\":\"User requested deletion\"");
        }

        [TestMethod]
        [Description("DELETE /v4/memories - Request with content match")]
        public void Serialize_WithContent_ProducesValidJson()
        {
            var request = new ForgetMemoryRequest
            {
                ContainerTag = "user_123",
                Content = "Some memory content to forget"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ForgetMemoryRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"content\":\"Some memory content to forget\"");
        }

        [TestMethod]
        [Description("DELETE /v4/memories - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_123",
                    "id": "mem_abc123",
                    "reason": "No longer relevant"
                }
                """;

            var request = JsonSerializer.Deserialize<ForgetMemoryRequest>(json, SupermemoryJsonContext.Default.ForgetMemoryRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_123");
            request.Id.Should().Be("mem_abc123");
            request.Reason.Should().Be("No longer relevant");
        }

        [TestMethod]
        [Description("DELETE /v4/memories - Deserialize minimal request")]
        public void Deserialize_MinimalRequest_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_456"
                }
                """;

            var request = JsonSerializer.Deserialize<ForgetMemoryRequest>(json, SupermemoryJsonContext.Default.ForgetMemoryRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_456");
            request.Id.Should().BeNull();
            request.Content.Should().BeNull();
        }

    }

}
