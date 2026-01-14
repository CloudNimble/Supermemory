using System.Text.Json;
using CloudNimble.Supermemory.Models.Memories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ForgetMemoryResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ForgetMemoryResponseTests
    {

        [TestMethod]
        [Description("DELETE /v4/memories - 200 response success")]
        public void Deserialize_200Success_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "mem_abc123",
                    "forgotten": true
                }
                """;

            var response = JsonSerializer.Deserialize<ForgetMemoryResponse>(json, SupermemoryJsonContext.Default.ForgetMemoryResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("mem_abc123");
            response.Forgotten.Should().BeTrue();
        }

        [TestMethod]
        [Description("DELETE /v4/memories - 200 response not forgotten")]
        public void Deserialize_200NotForgotten_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "mem_xyz789",
                    "forgotten": false
                }
                """;

            var response = JsonSerializer.Deserialize<ForgetMemoryResponse>(json, SupermemoryJsonContext.Default.ForgetMemoryResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("mem_xyz789");
            response.Forgotten.Should().BeFalse();
        }

    }

}
