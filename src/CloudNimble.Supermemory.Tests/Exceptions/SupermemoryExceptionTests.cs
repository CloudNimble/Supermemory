using System;
using CloudNimble.Supermemory.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Exceptions
{

    /// <summary>
    /// Tests for <see cref="SupermemoryException"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryExceptionTests
    {

        #region Constructor Tests

        [TestMethod]
        public void DefaultConstructor_CreatesException()
        {
            // Act
            var exception = new SupermemoryException();

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void MessageConstructor_SetsMessage()
        {
            // Arrange
            const string message = "Test error message";

            // Act
            var exception = new SupermemoryException(message);

            // Assert
            exception.Message.Should().Be(message);
        }

        [TestMethod]
        public void MessageAndInnerExceptionConstructor_SetsBoth()
        {
            // Arrange
            const string message = "Outer error";
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new SupermemoryException(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeSameAs(innerException);
        }

        #endregion

        #region Inheritance Tests

        [TestMethod]
        public void IsException()
        {
            // Act
            var exception = new SupermemoryException();

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }

        #endregion

    }

}
