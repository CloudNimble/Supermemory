using System;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Memories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="UpdateMemoryResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class UpdateMemoryResponseTests
    {

        [TestMethod]
        [Description("PATCH /v4/memories - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "mem_new123",
                    "memory": "Updated memory content with new information",
                    "version": 2,
                    "parentMemoryId": "mem_abc123",
                    "rootMemoryId": "mem_root001",
                    "createdAt": "2024-01-15T10:30:00.000Z"
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateMemoryResponse>(json, SupermemoryJsonContext.Default.UpdateMemoryResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("mem_new123");
            response.Memory.Should().Be("Updated memory content with new information");
            response.Version.Should().Be(2);
            response.ParentMemoryId.Should().Be("mem_abc123");
            response.RootMemoryId.Should().Be("mem_root001");
            response.CreatedAt.Should().Be(DateTimeOffset.Parse("2024-01-15T10:30:00.000Z"));
        }

        [TestMethod]
        [Description("PATCH /v4/memories - First version response")]
        public void Deserialize_FirstVersion_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "mem_first001",
                    "memory": "Initial memory content",
                    "version": 1,
                    "parentMemoryId": "",
                    "rootMemoryId": "mem_first001",
                    "createdAt": "2024-01-10T08:00:00.000Z"
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateMemoryResponse>(json, SupermemoryJsonContext.Default.UpdateMemoryResponse);

            response.Should().NotBeNull();
            response!.Version.Should().Be(1);
            response.ParentMemoryId.Should().BeEmpty();
            response.RootMemoryId.Should().Be("mem_first001");
        }

        [TestMethod]
        [Description("PATCH /v4/memories - Multiple version chain")]
        public void Deserialize_VersionChain_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "mem_v5",
                    "memory": "Fifth version of the memory",
                    "version": 5,
                    "parentMemoryId": "mem_v4",
                    "rootMemoryId": "mem_v1",
                    "createdAt": "2024-01-20T14:45:00.000Z"
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateMemoryResponse>(json, SupermemoryJsonContext.Default.UpdateMemoryResponse);

            response.Should().NotBeNull();
            response!.Version.Should().Be(5);
            response.ParentMemoryId.Should().Be("mem_v4");
            response.RootMemoryId.Should().Be("mem_v1");
        }

    }

}
