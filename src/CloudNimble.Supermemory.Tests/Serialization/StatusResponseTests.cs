using System.Text.Json;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="StatusResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class StatusResponseTests
    {

        [TestMethod]
        [Description("POST /v3/documents - 200 response sample")]
        public void Deserialize_AddDocument200_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "<string>",
                    "status": "<string>"
                }
                """;

            var response = JsonSerializer.Deserialize<StatusResponse>(json, SupermemoryJsonContext.Default.StatusResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("<string>");
            response.Status.Should().Be("<string>");
        }

        [TestMethod]
        [Description("PATCH /v3/documents/{id} - 200 response sample")]
        public void Deserialize_UpdateDocument200_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "doc_12345",
                    "status": "processing"
                }
                """;

            var response = JsonSerializer.Deserialize<StatusResponse>(json, SupermemoryJsonContext.Default.StatusResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("doc_12345");
            response.Status.Should().Be("processing");
        }

        [TestMethod]
        [Description("POST /v3/documents/file - 200 response sample")]
        public void Deserialize_UploadFile200_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "acxV5LHMEsG2hMSNb4umbn",
                    "status": "queued"
                }
                """;

            var response = JsonSerializer.Deserialize<StatusResponse>(json, SupermemoryJsonContext.Default.StatusResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("acxV5LHMEsG2hMSNb4umbn");
            response.Status.Should().Be("queued");
        }

    }

}
