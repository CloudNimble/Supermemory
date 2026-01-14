using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="BatchAddDocumentsRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class BatchAddDocumentsRequestTests
    {

        [TestMethod]
        [Description("POST /v3/documents/batch - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new BatchAddDocumentsRequest
            {
                Documents =
                [
                    new AddDocumentRequest
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
                    }
                ]
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.BatchAddDocumentsRequest);

            json.Should().Contain("\"documents\":");
            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"customId\":\"mem_abc123\"");
        }

        [TestMethod]
        [Description("POST /v3/documents/batch - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "documents": [
                        {
                            "content": "This is a detailed article about machine learning concepts...",
                            "containerTag": "user_123",
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
                    ]
                }
                """;

            var request = JsonSerializer.Deserialize<BatchAddDocumentsRequest>(json, SupermemoryJsonContext.Default.BatchAddDocumentsRequest);

            request.Should().NotBeNull();
            request!.Documents.Should().HaveCount(1);
            request.Documents[0].ContainerTag.Should().Be("user_123");
            request.Documents[0].CustomId.Should().Be("mem_abc123");
        }

    }

}
