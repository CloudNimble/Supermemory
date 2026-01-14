using System.Text.Json;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="DeleteBulkRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class DeleteBulkRequestTests
    {

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - Request body with IDs")]
        public void Serialize_WithIds_ProducesValidJson()
        {
            var request = new DeleteBulkRequest
            {
                Ids = ["doc_123", "doc_456", "doc_789"]
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.DeleteBulkRequest);

            json.Should().Contain("\"ids\":");
            json.Should().Contain("\"doc_123\"");
            json.Should().Contain("\"doc_456\"");
            json.Should().Contain("\"doc_789\"");
        }

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - Request body with container tags")]
        public void Serialize_WithContainerTags_ProducesValidJson()
        {
            var request = new DeleteBulkRequest
            {
                ContainerTags = ["user_123", "project_456"]
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.DeleteBulkRequest);

            json.Should().Contain("\"containerTags\":");
            json.Should().Contain("\"user_123\"");
            json.Should().Contain("\"project_456\"");
        }

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - Deserialize request with IDs")]
        public void Deserialize_WithIds_ReturnsValidObject()
        {
            var json = """
                {
                    "ids": ["doc_123", "doc_456", "doc_789"]
                }
                """;

            var request = JsonSerializer.Deserialize<DeleteBulkRequest>(json, SupermemoryJsonContext.Default.DeleteBulkRequest);

            request.Should().NotBeNull();
            request!.Ids.Should().HaveCount(3);
            request.Ids.Should().Contain("doc_123");
        }

        [TestMethod]
        [Description("DELETE /v3/documents/bulk - Deserialize request with container tags")]
        public void Deserialize_WithContainerTags_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTags": ["user_123", "project_456"]
                }
                """;

            var request = JsonSerializer.Deserialize<DeleteBulkRequest>(json, SupermemoryJsonContext.Default.DeleteBulkRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().HaveCount(2);
            request.ContainerTags.Should().Contain("user_123");
        }

    }

}
