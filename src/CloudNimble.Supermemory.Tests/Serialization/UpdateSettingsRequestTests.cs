using System.Text.Json;
using CloudNimble.Supermemory.Models.Settings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="UpdateSettingsRequest"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class UpdateSettingsRequestTests
    {

        [TestMethod]
        [Description("PATCH /v3/settings - Request body sample from API docs")]
        public void Serialize_ApiSample_ProducesValidJson()
        {
            var request = new UpdateSettingsRequest
            {
                ChunkSize = 1024,
                ShouldLlmFilter = true,
                FilterPrompt = "Remove any sensitive data"
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateSettingsRequest);

            json.Should().Contain("\"chunkSize\":1024");
            json.Should().Contain("\"shouldLLMFilter\":true");
            json.Should().Contain("\"filterPrompt\":\"Remove any sensitive data\"");
        }

        [TestMethod]
        [Description("PATCH /v3/settings - Request with OAuth settings")]
        public void Serialize_WithOAuthSettings_ProducesValidJson()
        {
            var request = new UpdateSettingsRequest
            {
                GitHubOAuthClientId = "new_github_client",
                GitHubOAuthClientSecret = "new_github_secret",
                GitHubOAuthCustomKeyEnabled = true
            };

            var json = JsonSerializer.Serialize(request, SupermemoryJsonContext.Default.UpdateSettingsRequest);

            json.Should().Contain("\"githubOAuthClientId\":\"new_github_client\"");
            json.Should().Contain("\"githubOAuthCustomKeyEnabled\":true");
        }

        [TestMethod]
        [Description("PATCH /v3/settings - Deserialize request sample")]
        public void Deserialize_ApiSample_ReturnsValidObject()
        {
            var json = """
                {
                    "chunkSize": 768,
                    "shouldLLMFilter": false,
                    "googleDriveOAuthCustomKeyEnabled": true,
                    "googleDriveOAuthClientId": "custom_google_id",
                    "googleDriveOAuthClientSecret": "custom_google_secret"
                }
                """;

            var request = JsonSerializer.Deserialize<UpdateSettingsRequest>(json, SupermemoryJsonContext.Default.UpdateSettingsRequest);

            request.Should().NotBeNull();
            request!.ChunkSize.Should().Be(768);
            request.ShouldLlmFilter.Should().BeFalse();
            request.GoogleDriveOAuthCustomKeyEnabled.Should().BeTrue();
        }

        [TestMethod]
        [Description("PATCH /v3/settings - Partial update request")]
        public void Deserialize_PartialUpdate_ReturnsValidObject()
        {
            var json = """
                {
                    "chunkSize": 512
                }
                """;

            var request = JsonSerializer.Deserialize<UpdateSettingsRequest>(json, SupermemoryJsonContext.Default.UpdateSettingsRequest);

            request.Should().NotBeNull();
            request!.ChunkSize.Should().Be(512);
            request.ShouldLlmFilter.Should().BeNull();
            request.FilterPrompt.Should().BeNull();
        }

    }

}
