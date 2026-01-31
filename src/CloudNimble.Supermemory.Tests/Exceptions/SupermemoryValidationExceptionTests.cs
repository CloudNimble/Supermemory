using System.Net;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryValidationException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryValidationExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithMessage_SetsMessageAndStatusCode()
        {
            // Arrange
            const string message = "Validation failed";

            // Act
            var exception = new SupermemoryValidationException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.ApiError.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_WithApiError_SetsApiError()
        {
            // Arrange
            const string message = "Invalid input";
            var apiError = new ApiError
            {
                Error = "validation_error",
                Message = "The 'content' field is required",
                Details = "Missing required field"
            };

            // Act
            var exception = new SupermemoryValidationException(message, apiError);

            // Assert
            exception.ApiError.Should().BeSameAs(apiError);
            exception.ApiError!.Details.Should().Be("Missing required field");
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsSupermemoryApiException()
        {
            // Act
            var exception = new SupermemoryValidationException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryApiException>();
        }

        [TestMethod]
        public void IsSupermemoryException()
        {
            // Act
            var exception = new SupermemoryValidationException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryException>();
        }

        #endregion

        #region StatusCode Tests

        [TestMethod]
        public void StatusCode_IsAlwaysBadRequest()
        {
            // Arrange
            var exception1 = new SupermemoryValidationException("Error 1");
            var exception2 = new SupermemoryValidationException("Error 2", new ApiError());

            // Assert
            exception1.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

    }

}
