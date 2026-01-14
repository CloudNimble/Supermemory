using System.Text.Json;
using CloudNimble.Supermemory.Models.Settings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SettingsResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class SettingsResponseTests
    {

        [TestMethod]
        [Description("GET /v3/settings - 200 response full sample")]
        public void Deserialize_200FullResponse_ReturnsValidObject()
        {
            var json = """
                {
                    "chunkSize": 512,
                    "filterPrompt": "Filter out personal information",
                    "shouldLLMFilter": true,
                    "githubOAuthClientId": "github_client_123",
                    "githubOAuthClientSecret": "github_secret_abc",
                    "githubOAuthCustomKeyEnabled": true,
                    "googleDriveOAuthClientId": "google_client_456",
                    "googleDriveOAuthClientSecret": "google_secret_def",
                    "googleDriveOAuthCustomKeyEnabled": false,
                    "notionOAuthClientId": "notion_client_789",
                    "notionOAuthClientSecret": "notion_secret_ghi",
                    "notionOAuthCustomKeyEnabled": true,
                    "onedriveOAuthClientId": "onedrive_client_012",
                    "onedriveOAuthClientSecret": "onedrive_secret_jkl",
                    "onedriveOAuthCustomKeyEnabled": false
                }
                """;

            var response = JsonSerializer.Deserialize<SettingsResponse>(json, SupermemoryJsonContext.Default.SettingsResponse);

            response.Should().NotBeNull();
            response!.ChunkSize.Should().Be(512);
            response.FilterPrompt.Should().Be("Filter out personal information");
            response.ShouldLlmFilter.Should().BeTrue();
            response.GitHubOAuthCustomKeyEnabled.Should().BeTrue();
            response.GoogleDriveOAuthCustomKeyEnabled.Should().BeFalse();
        }

        [TestMethod]
        [Description("GET /v3/settings - Minimal response")]
        public void Deserialize_MinimalResponse_ReturnsValidObject()
        {
            var json = """
                {
                    "chunkSize": 256
                }
                """;

            var response = JsonSerializer.Deserialize<SettingsResponse>(json, SupermemoryJsonContext.Default.SettingsResponse);

            response.Should().NotBeNull();
            response!.ChunkSize.Should().Be(256);
            response.FilterPrompt.Should().BeNull();
            response.ShouldLlmFilter.Should().BeNull();
        }

        [TestMethod]
        [Description("GET /v3/settings - Empty response")]
        public void Deserialize_EmptyResponse_ReturnsValidObject()
        {
            var json = """
                {}
                """;

            var response = JsonSerializer.Deserialize<SettingsResponse>(json, SupermemoryJsonContext.Default.SettingsResponse);

            response.Should().NotBeNull();
            response!.ChunkSize.Should().BeNull();
        }

    }

}
