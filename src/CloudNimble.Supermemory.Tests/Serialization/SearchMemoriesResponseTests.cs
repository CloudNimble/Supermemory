using System.Text.Json;
using CloudNimble.Supermemory.Models.Search;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SearchMemoriesResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class SearchMemoriesResponseTests
    {

        [TestMethod]
        [Description("POST /v4/search - 200 response with memory results")]
        public void Deserialize_MemoryResults_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "id": "mem_123",
                            "similarity": 0.95,
                            "updatedAt": "2024-01-15T10:30:00.000Z",
                            "memory": "User prefers dark mode and compact view",
                            "version": 2
                        }
                    ],
                    "timing": 35.2,
                    "total": 1
                }
                """;

            var response = JsonSerializer.Deserialize<SearchMemoriesResponse>(json, SupermemoryJsonContext.Default.SearchMemoriesResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(1);
            response.Timing.Should().Be(35.2);
            response.Total.Should().Be(1);
            response.Results[0].Id.Should().Be("mem_123");
            response.Results[0].Similarity.Should().Be(0.95);
            response.Results[0].Memory.Should().Be("User prefers dark mode and compact view");
            response.Results[0].Version.Should().Be(2);
        }

        [TestMethod]
        [Description("POST /v4/search - 200 response with hybrid results")]
        public void Deserialize_HybridResults_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "id": "chunk_456",
                            "similarity": 0.88,
                            "updatedAt": "2024-01-10T08:00:00.000Z",
                            "chunk": "This section discusses the implementation details..."
                        }
                    ],
                    "timing": 42.7,
                    "total": 1
                }
                """;

            var response = JsonSerializer.Deserialize<SearchMemoriesResponse>(json, SupermemoryJsonContext.Default.SearchMemoriesResponse);

            response.Should().NotBeNull();
            response!.Results[0].Chunk.Should().Contain("implementation details");
            response.Results[0].Memory.Should().BeNull();
        }

        [TestMethod]
        [Description("POST /v4/search - Empty results")]
        public void Deserialize_EmptyResults_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [],
                    "timing": 5.1,
                    "total": 0
                }
                """;

            var response = JsonSerializer.Deserialize<SearchMemoriesResponse>(json, SupermemoryJsonContext.Default.SearchMemoriesResponse);

            response.Should().NotBeNull();
            response!.Results.Should().BeEmpty();
            response.Total.Should().Be(0);
        }

        [TestMethod]
        [Description("POST /v4/search - Multiple results with metadata")]
        public void Deserialize_WithMetadata_ReturnsValidObject()
        {
            var json = """
                {
                    "results": [
                        {
                            "id": "mem_1",
                            "similarity": 0.92,
                            "updatedAt": "2024-01-01T00:00:00.000Z",
                            "memory": "First memory",
                            "metadata": {
                                "source": "conversation",
                                "importance": "high"
                            }
                        },
                        {
                            "id": "mem_2",
                            "similarity": 0.85,
                            "updatedAt": "2024-01-02T00:00:00.000Z",
                            "memory": "Second memory"
                        }
                    ],
                    "timing": 67.8,
                    "total": 2
                }
                """;

            var response = JsonSerializer.Deserialize<SearchMemoriesResponse>(json, SupermemoryJsonContext.Default.SearchMemoriesResponse);

            response.Should().NotBeNull();
            response!.Results.Should().HaveCount(2);
            response.Results[0].Metadata.Should().NotBeNull();
        }

    }

}
