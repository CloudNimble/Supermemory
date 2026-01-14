using System.Text.Json;
using CloudNimble.Supermemory.Models.Common;
using CloudNimble.Supermemory.Models.Documents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ListDocumentsRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ListDocumentsRequestTests
    {

        [TestMethod]
        [Description("POST /v3/documents/list - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new ListDocumentsRequest
            {
                ContainerTags = ["user_123"],
                Limit = 10,
                Page = 1,
                Sort = "createdAt",
                Order = "desc"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ListDocumentsRequest);

            json.Should().Contain("\"containerTags\":");
            json.Should().Contain("\"user_123\"");
            json.Should().Contain("\"limit\":10");
            json.Should().Contain("\"page\":1");
        }

        [TestMethod]
        [Description("POST /v3/documents/list - Request with filters")]
        public void Serialize_WithFilters_ProducesValidJson()
        {
            var request = new ListDocumentsRequest
            {
                ContainerTags = ["user_123"],
                Filters = new FilterExpression
                {
                    And =
                    [
                        new FilterCondition { Field = "category", Operator = "eq", Value = "technology" }
                    ]
                },
                IncludeContent = true
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ListDocumentsRequest);

            json.Should().Contain("\"filters\":");
            json.Should().Contain("\"includeContent\":true");
        }

        [TestMethod]
        [Description("POST /v3/documents/list - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTags": ["user_123", "project_456"],
                    "limit": 20,
                    "page": 2,
                    "sort": "updatedAt",
                    "order": "asc",
                    "includeContent": false
                }
                """;

            var request = JsonSerializer.Deserialize<ListDocumentsRequest>(json, SupermemoryJsonContext.Default.ListDocumentsRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().HaveCount(2);
            request.Limit.Should().Be(20);
            request.Page.Should().Be(2);
            request.Sort.Should().Be("updatedAt");
            request.Order.Should().Be("asc");
            request.IncludeContent.Should().BeFalse();
        }

    }

}
