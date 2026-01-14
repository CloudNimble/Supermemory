using System.Text.Json;
using CloudNimble.Supermemory.Models.Search;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SearchMemoriesRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class SearchMemoriesRequestTests
    {

        [TestMethod]
        [Description("POST /v4/search - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new SearchMemoriesRequest
            {
                Query = "user preferences",
                ContainerTag = "user_123",
                SearchMode = SearchMode.Memories,
                Limit = 10
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            json.Should().Contain("\"q\":\"user preferences\"");
            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"searchMode\":\"memories\"");
            json.Should().Contain("\"limit\":10");
        }

        [TestMethod]
        [Description("POST /v4/search - Request with hybrid mode")]
        public void Serialize_HybridMode_ProducesValidJson()
        {
            var request = new SearchMemoriesRequest
            {
                Query = "important notes",
                ContainerTag = "user_456",
                SearchMode = SearchMode.Hybrid,
                Rerank = true
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            json.Should().Contain("\"searchMode\":\"hybrid\"");
            json.Should().Contain("\"rerank\":true");
        }

        [TestMethod]
        [Description("POST /v4/search - Request with include options")]
        public void Serialize_WithInclude_ProducesValidJson()
        {
            var request = new SearchMemoriesRequest
            {
                Query = "recent activity",
                ContainerTag = "user_123",
                Include = new SearchMemoriesInclude
                {
                    Chunks = true,
                    Documents = true,
                    ForgottenMemories = false,
                    RelatedMemories = true,
                    Summaries = true
                }
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            json.Should().Contain("\"include\":");
            json.Should().Contain("\"chunks\":true");
            json.Should().Contain("\"documents\":true");
        }

        [TestMethod]
        [Description("POST /v4/search - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "q": "meeting notes",
                    "containerTag": "user_789",
                    "searchMode": "memories",
                    "limit": 15,
                    "rerank": true,
                    "include": {
                        "chunks": true,
                        "summaries": false
                    }
                }
                """;

            var request = JsonSerializer.Deserialize<SearchMemoriesRequest>(json, SupermemoryJsonContext.Default.SearchMemoriesRequest);

            request.Should().NotBeNull();
            request!.Query.Should().Be("meeting notes");
            request.ContainerTag.Should().Be("user_789");
            request.SearchMode.Should().Be(SearchMode.Memories);
            request.Limit.Should().Be(15);
            request.Rerank.Should().BeTrue();
            request.Include.Should().NotBeNull();
            request.Include!.Chunks.Should().BeTrue();
            request.Include.Summaries.Should().BeFalse();
        }

    }

}
