using System.Text.Json;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ApiError"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ApiErrorTests
    {

        [TestMethod]
        [Description("401 Unauthorized - Generic error sample")]
        public void Deserialize_401Generic_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "<string>",
                    "details": "<string>"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("<string>");
            error.Details.Should().Be("<string>");
        }

        [TestMethod]
        [Description("401 Unauthorized - Specific error sample")]
        public void Deserialize_401Specific_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "Invalid or missing authentication token",
                    "details": "Bearer token is required for this endpoint"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("Invalid or missing authentication token");
            error.Details.Should().Be("Bearer token is required for this endpoint");
        }

        [TestMethod]
        [Description("404 Not Found - Document not found sample")]
        public void Deserialize_404NotFound_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "Document not found",
                    "details": "The document with the specified ID does not exist"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("Document not found");
            error.Details.Should().Be("The document with the specified ID does not exist");
        }

        [TestMethod]
        [Description("500 Internal Server Error - Generic sample")]
        public void Deserialize_500Generic_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "Internal server error",
                    "details": "An unexpected error occurred while processing your request"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("Internal server error");
            error.Details.Should().Be("An unexpected error occurred while processing your request");
        }

        [TestMethod]
        [Description("400 Bad Request - Invalid parameters sample")]
        public void Deserialize_400BadRequest_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "Invalid request parameters",
                    "details": "Query must be at least 1 character long"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("Invalid request parameters");
            error.Details.Should().Be("Query must be at least 1 character long");
        }

        [TestMethod]
        [Description("Error with all optional fields")]
        public void Deserialize_WithAllFields_ReturnsValidObject()
        {
            var json = """
                {
                    "error": "Validation error",
                    "message": "The request body contains invalid data",
                    "code": "VALIDATION_ERROR",
                    "details": "Field 'content' is required"
                }
                """;

            var error = JsonSerializer.Deserialize<ApiError>(json, SupermemoryJsonContext.Default.ApiError);

            error.Should().NotBeNull();
            error!.Error.Should().Be("Validation error");
            error.Message.Should().Be("The request body contains invalid data");
            error.Code.Should().Be("VALIDATION_ERROR");
            error.Details.Should().Be("Field 'content' is required");
        }

    }

}
