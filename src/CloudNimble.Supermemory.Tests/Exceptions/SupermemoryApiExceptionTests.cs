using System;
using System.Net;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryApiException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryApiExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithMessageAndStatusCode_SetsProperties()
        {
            // Arrange
            const string message = "API error occurred";
            const HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            // Act
            var exception = new SupermemoryApiException(message, statusCode);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(statusCode);
            exception.ApiError.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_WithApiError_SetsApiError()
        {
            // Arrange
            const string message = "API error occurred";
            const HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            var apiError = new ApiError
            {
                Error = "validation_error",
                Message = "Invalid input",
                Code = "ERR001"
            };

            // Act
            var exception = new SupermemoryApiException(message, statusCode, apiError);

            // Assert
            exception.ApiError.Should().BeSameAs(apiError);
            exception.ApiError!.Error.Should().Be("validation_error");
            exception.ApiError.Message.Should().Be("Invalid input");
            exception.ApiError.Code.Should().Be("ERR001");
        }

        [TestMethod]
        public void Constructor_WithInnerException_SetsInnerException()
        {
            // Arrange
            const string message = "API error occurred";
            const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new SupermemoryApiException(message, statusCode, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(statusCode);
            exception.InnerException.Should().BeSameAs(innerException);
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsSupermemoryException()
        {
            // Act
            var exception = new SupermemoryApiException("Error", HttpStatusCode.BadRequest);

            // Assert
            exception.Should().BeAssignableTo<SupermemoryException>();
        }

        #endregion

        #region StatusCode Tests

        [TestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.Forbidden)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.ServiceUnavailable)]
        public void StatusCode_PreservesValue(HttpStatusCode statusCode)
        {
            // Act
            var exception = new SupermemoryApiException("Error", statusCode);

            // Assert
            exception.StatusCode.Should().Be(statusCode);
        }

        #endregion

    }

}
