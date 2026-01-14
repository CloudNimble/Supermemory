using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ListDocumentsResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ListDocumentsResponseTests
    {

        [TestMethod]
        [Description("POST /v3/documents/list - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "memories": [
                        {
                            "id": "doc_123",
                            "status": "done",
                            "type": "text",
                            "title": "Test Document",
                            "containerTags": ["user_123"]
                        }
                    ],
                    "pagination": {
                        "page": 1,
                        "totalPages": 5,
                        "total": 50,
                        "hasMore": true
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ListDocumentsResponse>(json, SupermemoryJsonContext.Default.ListDocumentsResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().HaveCount(1);
            response.Documents[0].Id.Should().Be("doc_123");
            response.Pagination.Should().NotBeNull();
            response.Pagination!.Page.Should().Be(1);
            response.Pagination.TotalPages.Should().Be(5);
            response.Pagination.Total.Should().Be(50);
            response.Pagination.HasMore.Should().BeTrue();
        }

        [TestMethod]
        [Description("POST /v3/documents/list - Empty results response")]
        public void Deserialize_EmptyResults_ReturnsValidObject()
        {
            var json = """
                {
                    "memories": [],
                    "pagination": {
                        "page": 1,
                        "totalPages": 0,
                        "total": 0,
                        "hasMore": false
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ListDocumentsResponse>(json, SupermemoryJsonContext.Default.ListDocumentsResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().BeEmpty();
            response.Pagination!.HasMore.Should().BeFalse();
        }

        [TestMethod]
        [Description("POST /v3/documents/list - Response with multiple documents")]
        public void Deserialize_MultipleDocuments_ReturnsValidObject()
        {
            var json = """
                {
                    "memories": [
                        {
                            "id": "doc_1",
                            "status": "done",
                            "type": "text"
                        },
                        {
                            "id": "doc_2",
                            "status": "queued",
                            "type": "webpage"
                        },
                        {
                            "id": "doc_3",
                            "status": "extracting",
                            "type": "pdf"
                        }
                    ],
                    "pagination": {
                        "page": 1,
                        "totalPages": 1,
                        "total": 3,
                        "hasMore": false
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ListDocumentsResponse>(json, SupermemoryJsonContext.Default.ListDocumentsResponse);

            response.Should().NotBeNull();
            response!.Documents.Should().HaveCount(3);
        }

    }

}
