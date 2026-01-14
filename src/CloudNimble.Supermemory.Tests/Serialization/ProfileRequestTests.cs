using System.Text.Json;
using CloudNimble.Supermemory.Models.Profile;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ProfileRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ProfileRequestTests
    {

        [TestMethod]
        [Description("POST /v4/profile - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new ProfileRequest
            {
                ContainerTag = "user_123",
                Query = "programming preferences",
                Threshold = 0.75
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ProfileRequest);

            json.Should().Contain("\"containerTag\":\"user_123\"");
            json.Should().Contain("\"q\":\"programming preferences\"");
            json.Should().Contain("\"threshold\":0.75");
        }

        [TestMethod]
        [Description("POST /v4/profile - Request without query")]
        public void Serialize_WithoutQuery_ProducesValidJson()
        {
            var request = new ProfileRequest
            {
                ContainerTag = "user_456"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.ProfileRequest);

            json.Should().Contain("\"containerTag\":\"user_456\"");
            json.Should().NotContain("\"q\":");
        }

        [TestMethod]
        [Description("POST /v4/profile - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_789",
                    "q": "favorite technologies",
                    "threshold": 0.8
                }
                """;

            var request = JsonSerializer.Deserialize<ProfileRequest>(json, SupermemoryJsonContext.Default.ProfileRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_789");
            request.Query.Should().Be("favorite technologies");
            request.Threshold.Should().Be(0.8);
        }

        [TestMethod]
        [Description("POST /v4/profile - Minimal request")]
        public void Deserialize_MinimalRequest_ReturnsValidObject()
        {
            var json = """
                {
                    "containerTag": "user_minimal"
                }
                """;

            var request = JsonSerializer.Deserialize<ProfileRequest>(json, SupermemoryJsonContext.Default.ProfileRequest);

            request.Should().NotBeNull();
            request!.ContainerTag.Should().Be("user_minimal");
            request.Query.Should().BeNull();
            request.Threshold.Should().BeNull();
        }

    }

}
