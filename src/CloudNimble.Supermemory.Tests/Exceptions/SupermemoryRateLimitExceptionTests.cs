using System.Net;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryRateLimitException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryRateLimitExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithMessage_SetsMessageAndStatusCode()
        {
            // Arrange
            const string message = "Rate limit exceeded";

            // Act
            var exception = new SupermemoryRateLimitException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
            exception.ApiError.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_WithApiError_SetsApiError()
        {
            // Arrange
            const string message = "Too many requests";
            var apiError = new ApiError
            {
                Error = "rate_limit_exceeded",
                Message = "Please wait before making more requests"
            };

            // Act
            var exception = new SupermemoryRateLimitException(message, apiError);

            // Assert
            exception.ApiError.Should().BeSameAs(apiError);
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsSupermemoryApiException()
        {
            // Act
            var exception = new SupermemoryRateLimitException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryApiException>();
        }

        [TestMethod]
        public void IsSupermemoryException()
        {
            // Act
            var exception = new SupermemoryRateLimitException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryException>();
        }

        #endregion

        #region StatusCode Tests

        [TestMethod]
        public void StatusCode_IsAlwaysTooManyRequests()
        {
            // Arrange
            var exception1 = new SupermemoryRateLimitException("Error 1");
            var exception2 = new SupermemoryRateLimitException("Error 2", new ApiError());

            // Assert
            exception1.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
            exception2.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        }

        #endregion

    }

}
