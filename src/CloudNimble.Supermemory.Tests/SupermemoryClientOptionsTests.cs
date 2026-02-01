using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CloudNimble.Supermemory.Tests
{

    /// <summary>
    /// Tests for <see cref="SupermemoryClientOptions"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryClientOptionsTests
    {

        #region Constructor Tests

        [TestMethod]
        public void DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var options = new SupermemoryClientOptions();

            // Assert
            options.BaseUrl.Should().Be(SupermemoryClientOptions.DefaultBaseUrl);
            options.Timeout.Should().Be(TimeSpan.FromSeconds(SupermemoryClientOptions.DefaultTimeoutSeconds));
            options.MaxRetries.Should().Be(SupermemoryClientOptions.DefaultMaxRetries);
            options.ApiKey.Should().BeNull();
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        public void Validate_ThrowsWhenApiKeyIsMissing()
        {
            // Arrange
            var options = new SupermemoryClientOptions();

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithParameterName("ApiKey");
        }

        [TestMethod]
        public void Validate_ThrowsWhenApiKeyIsWhitespace()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "   "
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithParameterName("ApiKey");
        }

        [TestMethod]
        public void Validate_DoesNotThrowWhenApiKeyIsProvided()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key"
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Validate_ThrowsWhenBaseUrlIsEmpty()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key",
                BaseUrl = string.Empty
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithParameterName("BaseUrl");
        }

        [TestMethod]
        public void Validate_ThrowsWhenTimeoutIsZero()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key",
                Timeout = TimeSpan.Zero
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("Timeout");
        }

        [TestMethod]
        public void Validate_ThrowsWhenTimeoutIsNegative()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key",
                Timeout = TimeSpan.FromSeconds(-1)
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("Timeout");
        }

        [TestMethod]
        public void Validate_ThrowsWhenMaxRetriesIsNegative()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key",
                MaxRetries = -1
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("MaxRetries");
        }

        [TestMethod]
        public void Validate_AllowsZeroMaxRetries()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                ApiKey = "test-key",
                MaxRetries = 0
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().NotThrow();
        }

        #endregion

        #region Constants Tests

        [TestMethod]
        public void Constants_HaveExpectedValues()
        {
            // Assert
            SupermemoryClientOptions.DefaultBaseUrl.Should().Be("https://api.supermemory.ai");
            SupermemoryClientOptions.DefaultTimeoutSeconds.Should().Be(60);
            SupermemoryClientOptions.DefaultMaxRetries.Should().Be(2);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void Properties_CanBeSetAndRead()
        {
            // Arrange & Act
            var options = new SupermemoryClientOptions
            {
                ApiKey = "my-api-key",
                BaseUrl = "https://custom.api.com",
                Timeout = TimeSpan.FromSeconds(120),
                MaxRetries = 5
            };

            // Assert
            options.ApiKey.Should().Be("my-api-key");
            options.BaseUrl.Should().Be("https://custom.api.com");
            options.Timeout.Should().Be(TimeSpan.FromSeconds(120));
            options.MaxRetries.Should().Be(5);
        }

        #endregion

    }

}
