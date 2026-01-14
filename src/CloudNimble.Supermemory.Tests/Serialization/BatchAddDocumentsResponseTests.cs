using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="BatchAddDocumentsResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class BatchAddDocumentsResponseTests
    {

        [TestMethod]
        [Description("POST /v3/documents/batch - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "id": "doc_123",
                            "status": "queued"
                        },
                        {
                            "id": "doc_456",
                            "status": "queued"
                        }
                    ]
                }
                """;

            var response = JsonSerializer.Deserialize<BatchAddDocumentsResponse>(json, SupermemoryJsonContext.Default.BatchAddDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(2);
            response.Results[0].Id.Should().Be("doc_123");
            response.Results[0].Status.Should().Be("queued");
            response.Results[1].Id.Should().Be("doc_456");
        }

        [TestMethod]
        [Description("POST /v3/documents/batch - Single document response")]
        public void Deserialize_SingleResult_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "id": "acxV5LHMEsG2hMSNb4umbn",
                            "status": "processing"
                        }
                    ]
                }
                """;

            var response = JsonSerializer.Deserialize<BatchAddDocumentsResponse>(json, SupermemoryJsonContext.Default.BatchAddDocumentsResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(1);
            response.Results[0].Id.Should().Be("acxV5LHMEsG2hMSNb4umbn");
        }

    }

}
