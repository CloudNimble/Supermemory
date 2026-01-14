using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

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
            options.HttpClient.Should().BeNull();
        }

        [TestMethod]
        public void ApiKeyConstructor_SetsApiKey()
        {
            // Arrange
            const string apiKey = "test-api-key";

            // Act
            var options = new SupermemoryClientOptions(apiKey);

            // Assert
            options.ApiKey.Should().Be(apiKey);
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        public void Validate_ThrowsWhenApiKeyAndHttpClientAreMissing()
        {
            // Arrange
            var options = new SupermemoryClientOptions();

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*API key*");
        }

        [TestMethod]
        public void Validate_DoesNotThrowWhenApiKeyIsProvided()
        {
            // Arrange
            var options = new SupermemoryClientOptions("test-key");

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Validate_DoesNotThrowWhenHttpClientIsProvided()
        {
            // Arrange
            var options = new SupermemoryClientOptions
            {
                HttpClient = new HttpClient()
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
            var options = new SupermemoryClientOptions("test-key")
            {
                BaseUrl = string.Empty
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*BaseUrl*");
        }

        [TestMethod]
        public void Validate_ThrowsWhenTimeoutIsZero()
        {
            // Arrange
            var options = new SupermemoryClientOptions("test-key")
            {
                Timeout = TimeSpan.Zero
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*Timeout*");
        }

        [TestMethod]
        public void Validate_ThrowsWhenMaxRetriesIsNegative()
        {
            // Arrange
            var options = new SupermemoryClientOptions("test-key")
            {
                MaxRetries = -1
            };

            // Act
            var action = () => options.Validate();

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*MaxRetries*");
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

    }

}
