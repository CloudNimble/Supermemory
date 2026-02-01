using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests
{

    /// <summary>
    /// Tests for <see cref="SupermemoryClient"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryClientTests
    {

        #region Helper Methods

        private static IOptions<SupermemoryClientOptions> CreateOptions(string apiKey = "test-api-key")
        {
            return Options.Create(new SupermemoryClientOptions
            {
                ApiKey = apiKey
            });
        }

        private static HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://api.supermemory.ai")
            };
        }

        #endregion

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithValidParameters_CreatesClient()
        {
            // Arrange
            var httpClient = CreateHttpClient();
            var options = CreateOptions();

            // Act
            var client = new SupermemoryClient(httpClient, options);

            // Assert
            client.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            // Arrange
            var options = CreateOptions();

            // Act
            var action = () => new SupermemoryClient(null!, options);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("httpClient");
        }

        [TestMethod]
        public void Constructor_WithNullOptions_ThrowsArgumentNullException()
        {
            // Arrange
            var httpClient = CreateHttpClient();

            // Act
            var action = () => new SupermemoryClient(httpClient, null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("options");
        }

        [TestMethod]
        public void Constructor_WithInvalidOptions_ThrowsArgumentException()
        {
            // Arrange
            var httpClient = CreateHttpClient();
            var options = Options.Create(new SupermemoryClientOptions()); // Missing ApiKey

            // Act
            var action = () => new SupermemoryClient(httpClient, options);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Resource Property Tests

        [TestMethod]
        public void Documents_ReturnsDocumentsResource()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var documents = client.Documents;

            // Assert
            documents.Should().NotBeNull();
        }

        [TestMethod]
        public void Documents_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

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
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var search = client.Search;

            // Assert
            search.Should().NotBeNull();
        }

        [TestMethod]
        public void Search_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

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
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var memories = client.Memories;

            // Assert
            memories.Should().NotBeNull();
        }

        [TestMethod]
        public void Memories_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

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
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var connections = client.Connections;

            // Assert
            connections.Should().NotBeNull();
        }

        [TestMethod]
        public void Connections_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

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
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var settings = client.Settings;

            // Assert
            settings.Should().NotBeNull();
        }

        [TestMethod]
        public void Settings_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

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
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var profile = client.Profile;

            // Assert
            profile.Should().NotBeNull();
        }

        [TestMethod]
        public void Profile_ReturnsSameInstance()
        {
            // Arrange
            var client = new SupermemoryClient(CreateHttpClient(), CreateOptions());

            // Act
            var profile1 = client.Profile;
            var profile2 = client.Profile;

            // Assert
            profile1.Should().BeSameAs(profile2);
        }

        #endregion

    }

}
