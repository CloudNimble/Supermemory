using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="DocumentResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class DocumentResponseTests
    {

        [TestMethod]
        [Description("GET /v3/documents/{id} - 200 response full sample")]
        public void Deserialize_GetDocument200_ReturnsValidObject()
        {
            var json = """
                {
                    "connectionId": "conn_123",
                    "containerTags": ["user_123"],
                    "content": "This is a detailed article about machine learning concepts...",
                    "createdAt": "1970-01-01T00:00:00.000Z",
                    "customId": "mem_abc123",
                    "id": "acxV5LHMEsG2hMSNb4umbn",
                    "metadata": {
                        "category": "technology",
                        "isPublic": true,
                        "readingTime": 5,
                        "source": "web",
                        "tag_1": "ai",
                        "tag_2": "machine-learning"
                    },
                    "ogImage": "https://example.com/image.jpg",
                    "raw": "This is a detailed article about machine learning concepts...",
                    "source": "web",
                    "status": "done",
                    "summary": "A comprehensive guide to understanding the basics of machine learning and its applications.",
                    "title": "Introduction to Machine Learning",
                    "tokenCount": 1000,
                    "type": "text",
                    "updatedAt": "1970-01-01T00:00:00.000Z",
                    "url": "https://example.com/article"
                }
                """;

            var response = JsonSerializer.Deserialize<DocumentResponse>(json, SupermemoryJsonContext.Default.DocumentResponse);

            response.Should().NotBeNull();
            response!.ConnectionId.Should().Be("conn_123");
            response.ContainerTags.Should().Contain("user_123");
            response.Content.Should().Contain("machine learning");
            response.CustomId.Should().Be("mem_abc123");
            response.Id.Should().Be("acxV5LHMEsG2hMSNb4umbn");
            response.OgImage.Should().Be("https://example.com/image.jpg");
            response.Source.Should().Be("web");
            response.Status.Should().Be(DocumentStatus.Done);
            response.Summary.Should().Contain("comprehensive guide");
            response.Title.Should().Be("Introduction to Machine Learning");
            response.TokenCount.Should().Be(1000);
            response.Type.Should().Be(DocumentType.Text);
            response.Url.Should().Be("https://example.com/article");
        }

        [TestMethod]
        [Description("GET /v3/documents/{id} - Minimal response")]
        public void Deserialize_MinimalResponse_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "doc_123",
                    "status": "queued",
                    "type": "webpage"
                }
                """;

            var response = JsonSerializer.Deserialize<DocumentResponse>(json, SupermemoryJsonContext.Default.DocumentResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("doc_123");
            response.Status.Should().Be(DocumentStatus.Queued);
            response.Type.Should().Be(DocumentType.Webpage);
        }

        [TestMethod]
        [Description("Document with google_doc type")]
        public void Deserialize_GoogleDocType_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "doc_456",
                    "status": "done",
                    "type": "google_doc",
                    "title": "My Google Doc"
                }
                """;

            var response = JsonSerializer.Deserialize<DocumentResponse>(json, SupermemoryJsonContext.Default.DocumentResponse);

            response.Should().NotBeNull();
            response!.Type.Should().Be(DocumentType.GoogleDoc);
        }

    }

}
