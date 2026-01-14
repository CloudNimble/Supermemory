using System.Text.Json;
using CloudNimble.Supermemory.Models.Connections;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="CreateConnectionResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class CreateConnectionResponseTests
    {

        [TestMethod]
        [Description("POST /v3/connections/{provider} - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_123abc",
                    "authLink": "https://accounts.google.com/oauth/authorize?...",
                    "expiresIn": 3600,
                    "redirectsTo": "https://example.com/callback"
                }
                """;

            var response = JsonSerializer.Deserialize<CreateConnectionResponse>(json, SupermemoryJsonContext.Default.CreateConnectionResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("conn_123abc");
            response.AuthLink.Should().Contain("accounts.google.com");
            response.ExpiresIn.Should().Be(3600);
            response.RedirectsTo.Should().Be("https://example.com/callback");
        }

        [TestMethod]
        [Description("POST /v3/connections/{provider} - Response without redirect")]
        public void Deserialize_WithoutRedirect_ReturnsValidObject()
        {
            var json = """
                {
                    "id": "conn_456def",
                    "authLink": "https://notion.so/oauth/authorize?...",
                    "expiresIn": 7200
                }
                """;

            var response = JsonSerializer.Deserialize<CreateConnectionResponse>(json, SupermemoryJsonContext.Default.CreateConnectionResponse);

            response.Should().NotBeNull();
            response!.Id.Should().Be("conn_456def");
            response.ExpiresIn.Should().Be(7200);
            response.RedirectsTo.Should().BeNull();
        }

    }

}
