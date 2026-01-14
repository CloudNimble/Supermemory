using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="UpdateDocumentRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class UpdateDocumentRequestTests
    {

        [TestMethod]
        [Description("PATCH /v3/documents/{id} - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new UpdateDocumentRequest
            {
                ContainerTag = "user_123",
                Content = "This is a detailed article about machine learning concepts...",
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

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateDocumentRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"customId\":\"mem_abc123\"");
        }

        [TestMethod]
        [Description("PATCH /v3/documents/{id} - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_123",
                    "containerTags": ["user_123", "project_123"],
                    "content": "This is a detailed article about machine learning concepts...",
                    "customId": "mem_abc123",
                    "metadata": {
                        "category": "technology",
                        "isPublic": true,
                        "readingTime": 5,
                        "source": "web",
                        "tag_1": "ai",
                        "tag_2": "machine-learning"
                    }
                }
                """;

            var request = JsonSerializer.Deserialize<UpdateDocumentRequest>(json, SupermemoryJsonContext.Default.UpdateDocumentRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_123");
            request.CustomId.Should().Be("mem_abc123");
        }

    }

}
