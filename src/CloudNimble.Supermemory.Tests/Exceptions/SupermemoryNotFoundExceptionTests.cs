using System.Net;
using CloudNimble.Supermemory.Exceptions;
using CloudNimble.Supermemory.Models.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryNotFoundException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryNotFoundExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithMessage_SetsMessageAndStatusCode()
        {
            // Arrange
            const string message = "Resource not found";

            // Act
            var exception = new SupermemoryNotFoundException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception.ApiError.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_WithApiError_SetsApiError()
        {
            // Arrange
            const string message = "Document not found";
            var apiError = new ApiError
            {
                Error = "not_found",
                Message = "The requested document does not exist"
            };

            // Act
            var exception = new SupermemoryNotFoundException(message, apiError);

            // Assert
            exception.ApiError.Should().BeSameAs(apiError);
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsSupermemoryApiException()
        {
            // Act
            var exception = new SupermemoryNotFoundException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryApiException>();
        }

        [TestMethod]
        public void IsSupermemoryException()
        {
            // Act
            var exception = new SupermemoryNotFoundException("Error");

            // Assert
            exception.Should().BeAssignableTo<SupermemoryException>();
        }

        #endregion

        #region StatusCode Tests

        [TestMethod]
        public void StatusCode_IsAlwaysNotFound()
        {
            // Arrange
            var exception1 = new SupermemoryNotFoundException("Error 1");
            var exception2 = new SupermemoryNotFoundException("Error 2", new ApiError());

            // Assert
            exception1.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception2.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

    }

}
