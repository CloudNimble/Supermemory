using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Documents;
using CloudNimble.Supermemory.Models.Memories;
using CloudNimble.Supermemory.Models.Search;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Tests for JSON serialization and deserialization using the AOT-compatible JSON context.
    /// </summary>
    [TestClass]
    public class JsonSerializationTests
    {

        #region Document Request Tests

        [TestMethod]
        public void AddDocumentRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new AddDocumentRequest
            {
                Content = "https://example.com",
                ContainerTag = "my-container",
                CustomId = "custom-123",
                Metadata = new Dictionary<string, object>
                {
                    ["key1"] = "value1",
                    ["key2"] = 42
                }
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.AddDocumentRequest);
            var deserialized = JsonSerializer.Deserialize<AddDocumentRequest>(json, SupermemoryJsonContext.Default.AddDocumentRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Content.Should().Be("https://example.com");
            deserialized.ContainerTag.Should().Be("my-container");
            deserialized.CustomId.Should().Be("custom-123");
        }

        [TestMethod]
        public void BatchAddDocumentsRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new BatchAddDocumentsRequest
            {
                Documents =
                [
                    new AddDocumentRequest { Content = "content1" },
                    new AddDocumentRequest { Content = "content2" }
                ]
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.BatchAddDocumentsRequest);
            var deserialized = JsonSerializer.Deserialize<BatchAddDocumentsRequest>(json, SupermemoryJsonContext.Default.BatchAddDocumentsRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Documents.Should().HaveCount(2);
        }

        [TestMethod]
        public void UpdateDocumentRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new UpdateDocumentRequest
            {
                Content = "updated content",
                ContainerTag = "updated-tag",
                CustomId = "custom-456"
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateDocumentRequest);
            var deserialized = JsonSerializer.Deserialize<UpdateDocumentRequest>(json, SupermemoryJsonContext.Default.UpdateDocumentRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Content.Should().Be("updated content");
            deserialized.ContainerTag.Should().Be("updated-tag");
        }

        [TestMethod]
        public void ListDocumentsRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new ListDocumentsRequest
            {
                Page = 2,
                Limit = 50,
                ContainerTags = ["tag1", "tag2"],
                Filters = new FilterExpression
                {
                    And =
                    [
                        new FilterCondition
                        {
                            Field = "status",
                            Operator = "eq",
                            Value = "done"
                        }
                    ]
                }
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ListDocumentsRequest);
            var deserialized = JsonSerializer.Deserialize<ListDocumentsRequest>(json, SupermemoryJsonContext.Default.ListDocumentsRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Page.Should().Be(2);
            deserialized.Limit.Should().Be(50);
            deserialized.Filters.Should().NotBeNull();
            deserialized.ContainerTags.Should().HaveCount(2);
        }

        #endregion

        #region Document Response Tests

        [TestMethod]
        public void DocumentResponse_DeserializesCorrectly()
        {
            // Arrange
            var json = """
                {
                    "id": "doc-123",
                    "status": "done",
                    "type": "webpage",
                    "url": "https://example.com",
                    "createdAt": "2024-01-15T10:30:00Z"
                }
                """;

            // Act
            var response = JsonSerializer.Deserialize<DocumentResponse>(json, SupermemoryJsonContext.Default.DocumentResponse);

            // Assert
            response.Should().NotBeNull();
            response!.Id.Should().Be("doc-123");
            response.Status.Should().Be(DocumentStatus.Done);
            response.Type.Should().Be(DocumentType.Webpage);
            response.Url.Should().Be("https://example.com");
        }

        [TestMethod]
        public void ListDocumentsResponse_DeserializesCorrectly()
        {
            // Arrange
            var json = """
                {
                    "memories": [
                        {"id": "doc-1", "status": "done"},
                        {"id": "doc-2", "status": "queued"}
                    ],
                    "pagination": {
                        "total": 100,
                        "page": 1,
                        "limit": 10,
                        "hasMore": true
                    }
                }
                """;

            // Act
            var response = JsonSerializer.Deserialize<ListDocumentsResponse>(json, SupermemoryJsonContext.Default.ListDocumentsResponse);

            // Assert
            response.Should().NotBeNull();
            response!.Documents.Should().HaveCount(2);
            response.Pagination.Should().NotBeNull();
            response.Pagination!.Total.Should().Be(100);
            response.Pagination.HasMore.Should().BeTrue();
        }

        #endregion

        #region Search Tests

        [TestMethod]
        public void SearchDocumentsRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new SearchDocumentsRequest
            {
                Query = "test query",
                Limit = 20,
                ContainerTags = ["container1"],
                Rerank = true
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchDocumentsRequest);
            var deserialized = JsonSerializer.Deserialize<SearchDocumentsRequest>(json, SupermemoryJsonContext.Default.SearchDocumentsRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Query.Should().Be("test query");
            deserialized.Limit.Should().Be(20);
            deserialized.Rerank.Should().BeTrue();
        }

        [TestMethod]
        public void SearchMemoriesRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new SearchMemoriesRequest
            {
                Query = "memory query",
                ContainerTag = "user-123",
                SearchMode = SearchMode.Hybrid,
                Include = new SearchMemoriesInclude
                {
                    Documents = true,
                    Chunks = false
                }
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchMemoriesRequest);
            var deserialized = JsonSerializer.Deserialize<SearchMemoriesRequest>(json, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Query.Should().Be("memory query");
            deserialized.SearchMode.Should().Be(SearchMode.Hybrid);
            deserialized.Include.Should().NotBeNull();
            deserialized.Include!.Documents.Should().BeTrue();
        }

        [TestMethod]
        public void SearchDocumentsResponse_DeserializesCorrectly()
        {
            // Arrange
            var json = """
                {
                    "results": [
                        {
                            "documentId": "doc-1",
                            "score": 0.95,
                            "content": "matched content",
                            "chunks": [
                                {"content": "chunk text", "score": 0.9, "isRelevant": true}
                            ]
                        }
                    ],
                    "timing": 150.5,
                    "total": 1
                }
                """;

            // Act
            var response = JsonSerializer.Deserialize<SearchDocumentsResponse>(json, SupermemoryJsonContext.Default.SearchDocumentsResponse);

            // Assert
            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(1);
            response.Results[0].Score.Should().Be(0.95);
            response.Results[0].DocumentId.Should().Be("doc-1");
            response.Timing.Should().Be(150.5);
        }

        #endregion

        #region Memory Tests

        [TestMethod]
        public void ForgetMemoryRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new ForgetMemoryRequest
            {
                ContainerTag = "user-123",
                Content = "forget this",
                Reason = "User requested deletion"
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ForgetMemoryRequest);
            var deserialized = JsonSerializer.Deserialize<ForgetMemoryRequest>(json, SupermemoryJsonContext.Default.ForgetMemoryRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Content.Should().Be("forget this");
            deserialized.ContainerTag.Should().Be("user-123");
            deserialized.Reason.Should().Be("User requested deletion");
        }

        [TestMethod]
        public void UpdateMemoryRequest_SerializesCorrectly()
        {
            // Arrange
            var request = new UpdateMemoryRequest
            {
                ContainerTag = "user-123",
                NewContent = "updated memory content",
                Content = "original content to find",
                Metadata = new Dictionary<string, object>
                {
                    ["field1"] = "new value"
                }
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateMemoryRequest);
            var deserialized = JsonSerializer.Deserialize<UpdateMemoryRequest>(json, SupermemoryJsonContext.Default.UpdateMemoryRequest);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.NewContent.Should().Be("updated memory content");
            deserialized.ContainerTag.Should().Be("user-123");
        }

        #endregion

        #region Common Types Tests

        [TestMethod]
        public void ApiError_DeserializesCorrectly()
        {
            // Arrange
            var json = """
                {
                    "error": "Something went wrong",
                    "message": "Detailed error message",
                    "code": "INVALID_REQUEST",
                    "details": "Additional details"
                }
                """;

            // Act
            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            // Assert
            error.Should().NotBeNull();
            error!.Error.Should().Be("Something went wrong");
            error.Message.Should().Be("Detailed error message");
            error.Code.Should().Be("INVALID_REQUEST");
        }

        [TestMethod]
        public void StatusResponse_DeserializesCorrectly()
        {
            // Arrange
            var json = """
                {
                    "id": "response-123",
                    "status": "completed"
                }
                """;

            // Act
            var response = JsonSerializer.Deserialize<StatusResponse>(json, SupermemoryJsonContext.Default.StatusResponse);

            // Assert
            response.Should().NotBeNull();
            response!.Id.Should().Be("response-123");
            response.Status.Should().Be("completed");
        }

        #endregion

        #region Enum Serialization Tests

        [TestMethod]
        public void DocumentStatus_SerializesAsString()
        {
            // Arrange
            var response = new DocumentResponse { Status = DocumentStatus.Done };

            // Act
            var json = JsonSerializer.Serialize(response, SupermemoryJsonContext.Default.DocumentResponse);

            // Assert
            json.Should().Contain("\"status\":\"done\"");
        }

        [TestMethod]
        public void DocumentType_SerializesAsString()
        {
            // Arrange
            var response = new DocumentResponse { Type = DocumentType.GoogleDoc };

            // Act
            var json = JsonSerializer.Serialize(response, SupermemoryJsonContext.Default.DocumentResponse);

            // Assert
            json.Should().Contain("\"type\":\"google_doc\"");
        }

        [TestMethod]
        public void SearchMode_SerializesAsString()
        {
            // Arrange
            var request = new SearchMemoriesRequest { SearchMode = SearchMode.Hybrid };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            // Assert
            json.Should().Contain("\"searchMode\":\"hybrid\"");
        }

        #endregion

        #region Null Handling Tests

        [TestMethod]
        public void NullProperties_AreOmittedWhenSerializing()
        {
            // Arrange
            var request = new AddDocumentRequest
            {
                Content = "test"
            };

            // Act
            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.AddDocumentRequest);

            // Assert
            json.Should().NotContain("\"metadata\"");
            json.Should().NotContain("\"containerTag\"");
        }

        #endregion

    }

}
