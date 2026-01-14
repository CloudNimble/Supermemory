using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="AddDocumentRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class AddDocumentRequestTests
    {

        [TestMethod]
        [Description("POST /v3/documents - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new AddDocumentRequest
            {
                Content = "sample content",
                ContainerTag = "user_123",
                CustomId = "doc_abc",
                Metadata = new Dictionary<string, object>()
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.AddDocumentRequest);

            json.Should().Contain("\"content\":\"sample content\"");
            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"customId\":\"doc_abc\"");
        }

        [TestMethod]
        [Description("POST /v3/documents - Request with full metadata sample")]
        public void Serialize_WithMetadata_ProducesValidJson()
        {
            var request = new AddDocumentRequest
            {
                Content = "This is a detailed article about machine learning concepts...",
                ContainerTag = "user_123",
                CustomId = "mem_abc123",
                Metadata = new Dictionary<string, object>
                {
                    ["category"] = "technology",
                    ["isPublic"] = true,
                    ["readingTime"] = 5,
                    ["source"] = "web",
                    ["tag_1"] = "ai",
                    ["tag_2"] = "machine-learning"
                }
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.AddDocumentRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"customId\":\"mem_abc123\"");
        }

        [TestMethod]
        [Description("POST /v3/documents - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "content": "<string>",
                    "containerTag": "<string>",
                    "customId": "<string>",
                    "metadata": {}
                }
                """;

            var request = JsonSerializer.Deserialize<AddDocumentRequest>(json, SupermemoryJsonContext.Default.AddDocumentRequest);

            request.Should().NotBeNull();
            request!.Content.Should().Be("<string>");
            request.ContainerTag.Should().Be("<string>");
            request.CustomId.Should().Be("<string>");
        }

    }

}
