using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ConfigureConnectionResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ConfigureConnectionResponseTests
    {

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - 200 response success")]
        public void Deserialize_200Success_ReturnsValidObject()
        {
            var json = """
                {
                    "message": "Connection configured successfully",
                    "success": true,
                    "webhooksRegistered": true
                }
                """;

            var response = JsonSerializer.Deserialize<ConfigureConnectionResponse>(json, SupermemoryJsonContext.Default.ConfigureConnectionResponse);

            response.Should().NotBeNull();
            response!.Message.Should().Be("Connection configured successfully");
            response.Success.Should().BeTrue();
            response.WebhooksRegistered.Should().BeTrue();
        }

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - 200 response without webhooks")]
        public void Deserialize_WithoutWebhooks_ReturnsValidObject()
        {
            var json = """
                {
                    "message": "Resources updated",
                    "success": true
                }
                """;

            var response = JsonSerializer.Deserialize<ConfigureConnectionResponse>(json, SupermemoryJsonContext.Default.ConfigureConnectionResponse);

            response.Should().NotBeNull();
            response!.Success.Should().BeTrue();
            response.WebhooksRegistered.Should().BeNull();
        }

        [TestMethod]
        [Description("POST /v3/connections/{id}/configure - Partial success")]
        public void Deserialize_PartialSuccess_ReturnsValidObject()
        {
            var json = """
                {
                    "message": "Some resources could not be configured",
                    "success": false,
                    "webhooksRegistered": false
                }
                """;

            var response = JsonSerializer.Deserialize<ConfigureConnectionResponse>(json, SupermemoryJsonContext.Default.ConfigureConnectionResponse);

            response.Should().NotBeNull();
            response!.Success.Should().BeFalse();
            response.WebhooksRegistered.Should().BeFalse();
        }

    }

}
