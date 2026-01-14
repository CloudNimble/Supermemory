using System.Collections.Generic;
using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="CreateConnectionRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class CreateConnectionRequestTests
    {

        [TestMethod]
        [Description("POST /v3/connections/{provider} - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new CreateConnectionRequest
            {
                ContainerTags = ["user_123"],
                DocumentLimit = 100,
                RedirectUrl = "https://example.com/callback"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.CreateConnectionRequest);

            json.Should().Contain("\"containerTags\":");
            json.Should().Contain("\"user_123\"");
            json.Should().Contain("\"documentLimit\":100");
            json.Should().Contain("\"redirectUrl\":\"https://example.com/callback\"");
        }

        [TestMethod]
        [Description("POST /v3/connections/{provider} - Request with metadata")]
        public void Serialize_WithMetadata_ProducesValidJson()
        {
            var request = new CreateConnectionRequest
            {
                ContainerTags = ["user_456", "project_789"],
                Metadata = new Dictionary<string, object>
                {
                    ["purpose"] = "sync",
                    ["priority"] = 1
                }
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.CreateConnectionRequest);

            json.Should().Contain("\"metadata\":");
            json.Should().Contain("\"containerTags\":");
        }

        [TestMethod]
        [Description("POST /v3/connections/{provider} - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTags": ["user_123", "team_456"],
                    "documentLimit": 500,
                    "redirectUrl": "https://app.example.com/oauth/callback",
                    "metadata": {
                        "department": "engineering"
                    }
                }
                """;

            var request = JsonSerializer.Deserialize<CreateConnectionRequest>(json, SupermemoryJsonContext.Default.CreateConnectionRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().HaveCount(2);
            request.DocumentLimit.Should().Be(500);
            request.RedirectUrl.Should().Be("https://app.example.com/oauth/callback");
            request.Metadata.Should().NotBeNull();
        }

        [TestMethod]
        [Description("POST /v3/connections/{provider} - Minimal request")]
        public void Deserialize_MinimalRequest_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTags": ["user_only"]
                }
                """;

            var request = JsonSerializer.Deserialize<CreateConnectionRequest>(json, SupermemoryJsonContext.Default.CreateConnectionRequest);

            request.Should().NotBeNull();
            request!.ContainerTags.Should().Contain("user_only");
            request.DocumentLimit.Should().BeNull();
            request.RedirectUrl.Should().BeNull();
        }

    }

}
