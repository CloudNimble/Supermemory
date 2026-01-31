using System.Net;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryAuthenticationException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryAuthenticationExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithMessage_SetsMessageAndStatusCode()
        {
            // Arrange
            const string message = "Authentication failed";

            // Act
            var exception = new SupermemoryAuthenticationException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            exception.ApiError.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_WithApiError_SetsApiError()
        {
            // Arrange
            const string message = "Authentication failed";
            var apiError = new ApiError
            {
                Error = "unauthorized",
                Message = "Invalid API key"
            };

            // Act
            var exception = new SupermemoryAuthenticationException(message, apiError);

            // Assert
            exception.ApiError.Should().BeSameAs(apiError);
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsSupermemoryApiException()
        {
            // Act
            var exception = new SupermemoryAuthenticationException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryApiException>();
        }

        [TestMethod]
        public void IsSupermemoryException()
        {
            // Act
            var exception = new SupermemoryAuthenticationException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryException>();
        }

        #endregion

        #region StatusCode Tests

        [TestMethod]
        public void StatusCode_IsAlwaysUnauthorized()
        {
            // Arrange
            var exception1 = new SupermemoryAuthenticationException("Error 1");
            var exception2 = new SupermemoryAuthenticationException("Error 2", new ApiError());

            // Assert
            exception1.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            exception2.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        #endregion

    }

}
