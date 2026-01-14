using System.Text.Json;
using CloudNimble.Supermemory.Models.Search;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SearchDocumentsResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class SearchDocumentsResponseTests
    {

        [TestMethod]
        [Description("POST /v3/search - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "documentId": "doc_123",
                            "title": "Introduction to Machine Learning",
                            "type": "text",
                            "score": 0.95,
                            "content": "Machine learning is a subset of artificial intelligence...",
                            "summary": "An overview of ML concepts",
                            "createdAt": "2024-01-10T08:00:00.000Z",
                            "updatedAt": "2024-01-15T10:30:00.000Z"
                        }
                    ],
                    "timing": 45.5,
                    "total": 1
                }
                """;

            var response = JsonSerializer.Deserialize<SearchDocumentsResponse>(json, SupermemoryJsonContext.Default.SearchDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(1);
            response.Timing.Should().Be(45.5);
            response.Total.Should().Be(1);
            response.Results[0].DocumentId.Should().Be("doc_123");
            response.Results[0].Title.Should().Be("Introduction to Machine Learning");
            response.Results[0].Type.Should().Be(DocumentType.Text);
            response.Results[0].Score.Should().Be(0.95);
        }

        [TestMethod]
        [Description("POST /v3/search - Response with chunks")]
        public void Deserialize_WithChunks_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "documentId": "doc_456",
                            "title": "AI Research Paper",
                            "type": "pdf",
                            "score": 0.88,
                            "createdAt": "2024-01-01T00:00:00.000Z",
                            "updatedAt": "2024-01-01T00:00:00.000Z",
                            "chunks": [
                                {
                                    "content": "This section discusses neural networks...",
                                    "isRelevant": true,
                                    "score": 0.92
                                },
                                {
                                    "content": "Deep learning architectures...",
                                    "isRelevant": true,
                                    "score": 0.85
                                }
                            ]
                        }
                    ],
                    "timing": 120.3,
                    "total": 1
                }
                """;

            var response = JsonSerializer.Deserialize<SearchDocumentsResponse>(json, SupermemoryJsonContext.Default.SearchDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results[0].Chunks.Should().HaveCount(2);
            response.Results[0].Chunks![0].IsRelevant.Should().BeTrue();
            response.Results[0].Chunks![0].Score.Should().Be(0.92);
        }

        [TestMethod]
        [Description("POST /v3/search - Empty results")]
        public void Deserialize_EmptyResults_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [],
                    "timing": 10.2,
                    "total": 0
                }
                """;

            var response = JsonSerializer.Deserialize<SearchDocumentsResponse>(json, SupermemoryJsonContext.Default.SearchDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results.Should().BeEmpty();
            response.Total.Should().Be(0);
        }

        [TestMethod]
        [Description("POST /v3/search - Multiple results")]
        public void Deserialize_MultipleResults_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "documentId": "doc_1",
                            "type": "text",
                            "score": 0.95,
                            "createdAt": "2024-01-01T00:00:00.000Z",
                            "updatedAt": "2024-01-01T00:00:00.000Z"
                        },
                        {
                            "documentId": "doc_2",
                            "type": "webpage",
                            "score": 0.87,
                            "createdAt": "2024-01-01T00:00:00.000Z",
                            "updatedAt": "2024-01-01T00:00:00.000Z"
                        },
                        {
                            "documentId": "doc_3",
                            "type": "pdf",
                            "score": 0.75,
                            "createdAt": "2024-01-01T00:00:00.000Z",
                            "updatedAt": "2024-01-01T00:00:00.000Z"
                        }
                    ],
                    "timing": 55.0,
                    "total": 3
                }
                """;

            var response = JsonSerializer.Deserialize<SearchDocumentsResponse>(json, SupermemoryJsonContext.Default.SearchDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(3);
            response.Total.Should().Be(3);
        }

    }

}
