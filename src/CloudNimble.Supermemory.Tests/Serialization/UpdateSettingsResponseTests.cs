using System.Text.Json;
using CloudNimble.Supermemory.Models.Settings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="UpdateSettingsResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class UpdateSettingsResponseTests
    {

        [TestMethod]
        [Description("PATCH /v3/settings - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "orgId": "org_123abc",
                    "orgSlug": "my-organization",
                    "updated": {
                        "chunkSize": 1024,
                        "shouldLLMFilter": true,
                        "filterPrompt": "Updated filter prompt"
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateSettingsResponse>(json, SupermemoryJsonContext.Default.UpdateSettingsResponse);

            response.Should().NotBeNull();
            response!.OrgId.Should().Be("org_123abc");
            response.OrgSlug.Should().Be("my-organization");
            response.Updated.Should().NotBeNull();
            response.Updated!.ChunkSize.Should().Be(1024);
            response.Updated.ShouldLlmFilter.Should().BeTrue();
        }

        [TestMethod]
        [Description("PATCH /v3/settings - Response with OAuth updates")]
        public void Deserialize_WithOAuthUpdates_ReturnsValidObject()
        {
            var json = """
                {
                    "orgId": "org_456def",
                    "orgSlug": "tech-company",
                    "updated": {
                        "githubOAuthCustomKeyEnabled": true,
                        "notionOAuthCustomKeyEnabled": false
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateSettingsResponse>(json, SupermemoryJsonContext.Default.UpdateSettingsResponse);

            response.Should().NotBeNull();
            response!.Updated!.GitHubOAuthCustomKeyEnabled.Should().BeTrue();
            response.Updated.NotionOAuthCustomKeyEnabled.Should().BeFalse();
        }

        [TestMethod]
        [Description("PATCH /v3/settings - Response without updated settings")]
        public void Deserialize_WithoutUpdated_ReturnsValidObject()
        {
            var json = """
                {
                    "orgId": "org_789ghi",
                    "orgSlug": "another-org"
                }
                """;

            var response = JsonSerializer.Deserialize<UpdateSettingsResponse>(json, SupermemoryJsonContext.Default.UpdateSettingsResponse);

            response.Should().NotBeNull();
            response!.OrgId.Should().Be("org_789ghi");
            response.Updated.Should().BeNull();
        }

    }

}
