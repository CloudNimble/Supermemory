using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Memories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="UpdateMemoryRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class UpdateMemoryRequestTests
    {

        [TestMethod]
        [Description("PATCH /v4/memories - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new UpdateMemoryRequest
            {
                ContainerTag = "user_123",
                NewContent = "Updated memory content with new information",
                Id = "mem_abc123"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateMemoryRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"newContent\":\"Updated memory content with new information\"");
            json.Should().Contain("\"id\":\"mem_abc123\"");
        }

        [TestMethod]
        [Description("PATCH /v4/memories - Request with content match")]
        public void Serialize_WithContentMatch_ProducesValidJson()
        {
            var request = new UpdateMemoryRequest
            {
                ContainerTag = "user_123",
                Content = "Original content to find",
                NewContent = "New replacement content"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateMemoryRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"content\":\"Original content to find\"");
            json.Should().Contain("\"newContent\":\"New replacement content\"");
        }

        [TestMethod]
        [Description("PATCH /v4/memories - Request with metadata")]
        public void Serialize_WithMetadata_ProducesValidJson()
        {
            var request = new UpdateMemoryRequest
            {
                ContainerTag = "user_123",
                NewContent = "Updated content",
                Id = "mem_abc123",
                Metadata = new Dictionary<string, object>
                {
                    ["category"] = "updated",
                    ["priority"] = 1
                }
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateMemoryRequest);

            json.Should().Contain("\"metadata\":");
            json.Should().Contain("\"category\"");
        }

        [TestMethod]
        [Description("PATCH /v4/memories - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_123",
                    "newContent": "Updated memory content",
                    "id": "mem_abc123",
                    "content": "Original content"
                }
                """;

            var request = JsonSerializer.Deserialize<UpdateMemoryRequest>(json, SupermemoryJsonContext.Default.UpdateMemoryRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_123");
            request.NewContent.Should().Be("Updated memory content");
            request.Id.Should().Be("mem_abc123");
            request.Content.Should().Be("Original content");
        }

    }

}
