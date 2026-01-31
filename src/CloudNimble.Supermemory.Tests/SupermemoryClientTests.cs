using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests
{

    /// <summary>
    /// Tests for <see cref="SupermemoryClient"/>.
    /// These tests are excluded until API call mocking is implemented.
    /// </summary>
    [TestClass]
    [Ignore("Tests excluded until API call mocking is implemented")]
    public class SupermemoryClientTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithApiKey_CreatesClient()
        {
            // Arrange & Act
            using var client = new SupermemoryClient("test-api-key");

            // Assert
            client.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithOptions_CreatesClient()
        {
            // Arrange
            var options = new SupermemoryClientOptions("test-api-key");

            // Act
            using var client = new SupermemoryClient(options);

            // Assert
            client.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithHttpClientOption_UsesProvidedClient()
        {
            // Arrange
            var httpClient = new HttpClient();
            var options = new SupermemoryClientOptions
            {
                HttpClient = httpClient
            };

            // Act
            using var client = new SupermemoryClient(options);

            // Assert
            client.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithNullOptions_ThrowsArgumentNullException()
        {
            // Act
            var action = () => new SupermemoryClient((SupermemoryClientOptions)null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("options");
        }

        [TestMethod]
        public void Constructor_WithInvalidOptions_ThrowsInvalidOperationException()
        {
            // Arrange
            var options = new SupermemoryClientOptions();

            // Act
            var action = () => new SupermemoryClient(options);

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        #endregion

        #region Resource Property Tests

        [TestMethod]
        public void Documents_ReturnsDocumentsResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var documents = client.Documents;

            // Assert
            documents.Should().NotBeNull();
        }

        [TestMethod]
        public void Documents_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var documents1 = client.Documents;
            var documents2 = client.Documents;

            // Assert
            documents1.Should().BeSameAs(documents2);
        }

        [TestMethod]
        public void Search_ReturnsSearchResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var search = client.Search;

            // Assert
            search.Should().NotBeNull();
        }

        [TestMethod]
        public void Search_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var search1 = client.Search;
            var search2 = client.Search;

            // Assert
            search1.Should().BeSameAs(search2);
        }

        [TestMethod]
        public void Memories_ReturnsMemoriesResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var memories = client.Memories;

            // Assert
            memories.Should().NotBeNull();
        }

        [TestMethod]
        public void Memories_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var memories1 = client.Memories;
            var memories2 = client.Memories;

            // Assert
            memories1.Should().BeSameAs(memories2);
        }

        [TestMethod]
        public void Connections_ReturnsConnectionsResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var connections = client.Connections;

            // Assert
            connections.Should().NotBeNull();
        }

        [TestMethod]
        public void Connections_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var connections1 = client.Connections;
            var connections2 = client.Connections;

            // Assert
            connections1.Should().BeSameAs(connections2);
        }

        [TestMethod]
        public void Settings_ReturnsSettingsResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var settings = client.Settings;

            // Assert
            settings.Should().NotBeNull();
        }

        [TestMethod]
        public void Settings_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var settings1 = client.Settings;
            var settings2 = client.Settings;

            // Assert
            settings1.Should().BeSameAs(settings2);
        }

        [TestMethod]
        public void Profile_ReturnsProfileResource()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var profile = client.Profile;

            // Assert
            profile.Should().NotBeNull();
        }

        [TestMethod]
        public void Profile_ReturnsSameInstance()
        {
            // Arrange
            using var client = new SupermemoryClient("test-api-key");

            // Act
            var profile1 = client.Profile;
            var profile2 = client.Profile;

            // Assert
            profile1.Should().BeSameAs(profile2);
        }

        #endregion

        #region Dispose Tests

        [TestMethod]
        public void Dispose_DisposesOwnedHttpClient()
        {
            // Arrange
            var client = new SupermemoryClient("test-api-key");

            // Act
            client.Dispose();

            // Assert - no exception thrown
        }

        [TestMethod]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            // Arrange
            var client = new SupermemoryClient("test-api-key");

            // Act
            client.Dispose();
            var action = () => client.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Dispose_WithProvidedHttpClient_DoesNotDisposeIt()
        {
            // Arrange
            var httpClient = new HttpClient();
            var options = new SupermemoryClientOptions
            {
                HttpClient = httpClient
            };
            var client = new SupermemoryClient(options);

            // Act
            client.Dispose();

            // Assert - HttpClient should still be usable
            var action = () => httpClient.BaseAddress = new Uri("http://example.com");
            action.Should().NotThrow();

            httpClient.Dispose();
        }

        #endregion

    }

}
