using System.Text.Json;
using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Search;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SearchDocumentsRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class SearchDocumentsRequestTests
    {

        [TestMethod]
        [Description("POST /v3/search - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new SearchDocumentsRequest
            {
                Query = "machine learning concepts",
                Limit = 10,
                Rerank = true
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchDocumentsRequest);

            json.Should().Contain("\"q\":\"machine learning concepts\"");
            json.Should().Contain("\"limit\":10");
            json.Should().Contain("\"rerank\":true");
        }

        [TestMethod]
        [Description("POST /v3/search - Request with container tags")]
        public void Serialize_WithContainerTags_ProducesValidJson()
        {
            var request = new SearchDocumentsRequest
            {
                Query = "technology articles",
                ContainerTags = ["user_123", "project_456"],
                Limit = 5
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchDocumentsRequest);

            json.Should().Contain("\"q\":\"technology articles\"");
            json.Should().Contain("\"containerTags\":");
            json.Should().Contain("\"user_123\"");
        }

        [TestMethod]
        [Description("POST /v3/search - Request with filters")]
        public void Serialize_WithFilters_ProducesValidJson()
        {
            var request = new SearchDocumentsRequest
            {
                Query = "AI research",
                Filters = new FilterExpression
                {
                    And =
                    [
                        new FilterCondition { Field = "category", Operator = "eq", Value = "technology" }
                    ]
                },
                RewriteQuery = true
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.SearchDocumentsRequest);

            json.Should().Contain("\"filters\":");
            json.Should().Contain("\"rewriteQuery\":true");
        }

        [TestMethod]
        [Description("POST /v3/search - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "q": "machine learning",
                    "limit": 20,
                    "rerank": false,
                    "rewriteQuery": true,
                    "containerTags": ["user_123"]
                }
                """;

            var request = JsonSerializer.Deserialize<SearchDocumentsRequest>(json, SupermemoryJsonContext.Default.SearchDocumentsRequest);

            request.Should().NotBeNull();
            request!.Query.Should().Be("machine learning");
            request.Limit.Should().Be(20);
            request.Rerank.Should().BeFalse();
            request.RewriteQuery.Should().BeTrue();
            request.ContainerTags.Should().Contain("user_123");
        }

    }

}
