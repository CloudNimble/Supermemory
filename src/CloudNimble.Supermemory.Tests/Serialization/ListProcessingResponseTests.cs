using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ListProcessingResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ListProcessingResponseTests
    {

        [TestMethod]
        [Description("GET /v3/documents/processing - 200 response sample")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "documents": [
                        {
                            "id": "doc_123",
                            "status": "extracting",
                            "type": "pdf",
                            "title": "Processing Document"
                        },
                        {
                            "id": "doc_456",
                            "status": "embedding",
                            "type": "webpage"
                        }
                    ],
                    "totalCount": 2
                }
                """;

            var response = JsonSerializer.Deserialize<ListProcessingResponse>(json, SupermemoryJsonContext.Default.ListProcessingResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().HaveCount(2);
            response.TotalCount.Should().Be(2);
            response.Documents[0].Id.Should().Be("doc_123");
        }

        [TestMethod]
        [Description("GET /v3/documents/processing - Empty response")]
        public void Deserialize_EmptyResponse_ReturnsValidObject()
        {
            var json = """
                {
                    "documents": [],
                    "totalCount": 0
                }
                """;

            var response = JsonSerializer.Deserialize<ListProcessingResponse>(json, SupermemoryJsonContext.Default.ListProcessingResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().BeEmpty();
            response.TotalCount.Should().Be(0);
        }

        [TestMethod]
        [Description("GET /v3/documents/processing - Various processing states")]
        public void Deserialize_VariousStates_ReturnsValidObject()
        {
            var json = """
                {
                    "documents": [
                        {
                            "id": "doc_1",
                            "status": "queued",
                            "type": "text"
                        },
                        {
                            "id": "doc_2",
                            "status": "chunking",
                            "type": "pdf"
                        },
                        {
                            "id": "doc_3",
                            "status": "indexing",
                            "type": "webpage"
                        }
                    ],
                    "totalCount": 3
                }
                """;

            var response = JsonSerializer.Deserialize<ListProcessingResponse>(json, SupermemoryJsonContext.Default.ListProcessingResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().HaveCount(3);
            response.TotalCount.Should().Be(3);
        }

    }

}
