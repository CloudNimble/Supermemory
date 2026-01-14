using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="DeleteBulkResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class DeleteBulkResponseTests
    {

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - 200 response success")]
        public void Deserialize_200Success_ReturnsValidObject()
        {
            var json = """
                {
                    "success": true,
                    "deletedCount": 3
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteBulkResponse>(json, SupermemoryJsonContext.Default.DeleteBulkResponse);

            response.Should().NotBeNull();
            response!.Success.Should().BeTrue();
            response.DeletedCount.Should().Be(3);
        }

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - 200 response with container tags")]
        public void Deserialize_200WithContainerTags_ReturnsValidObject()
        {
            var json = """
                {
                    "success": true,
                    "deletedCount": 5,
                    "containerTags": ["user_123", "project_456"]
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteBulkResponse>(json, SupermemoryJsonContext.Default.DeleteBulkResponse);

            response.Should().NotBeNull();
            response!.Success.Should().BeTrue();
            response.DeletedCount.Should().Be(5);
            response.ContainerTags.Should().HaveCount(2);
        }

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - 200 response with errors")]
        public void Deserialize_200WithErrors_ReturnsValidObject()
        {
            var json = """
                {
                    "success": false,
                    "deletedCount": 2,
                    "errors": [
                        {
                            "id": "doc_789",
                            "error": "Document not found"
                        }
                    ]
                }
                """;

            var response = JsonSerializer.Deserialize<DeleteBulkResponse>(json, SupermemoryJsonContext.Default.DeleteBulkResponse);

            response.Should().NotBeNull();
            response!.Success.Should().BeFalse();
            response.DeletedCount.Should().Be(2);
            response.Errors.Should().HaveCount(1);
            response.Errors![0].Id.Should().Be("doc_789");
            response.Errors[0].Error.Should().Be("Document not found");
        }

    }

}
