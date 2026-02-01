using System.Net.Http;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using FluentAssertions;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory.Extensions
{

    /// <summary>
    /// Tests for Supermemory AgentOptions extensions.
    /// These tests are excluded until API call mocking is implemented.
    /// </summary>
    [TestClass]
    [Ignore("Tests excluded until API call mocking is implemented")]
    public class AgentOptionsExtensionsTests
    {

        #region WithSupermemoryContext Tests

        [TestMethod]
        public void WithSupermemoryContext_SetsAIContextProviderFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryContext(client);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemoryContext_WithNullOptions_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            ChatClientAgentOptions? options = null;

            // Act
            var action = () => options!.WithSupermemoryContext(client);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("options");
        }

        [TestMethod]
        public void WithSupermemoryContext_WithNullClient_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new ChatClientAgentOptions();

            // Act
            var action = () => options.WithSupermemoryContext(null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [TestMethod]
        public void WithSupermemoryContext_WithProviderOptions_SetsFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var providerOptions = new SupermemoryContextProviderOptions { DefaultContainerTag = "test" };
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryContext(client, providerOptions);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemoryContext_WithResolver_SetsFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var resolver = new TestContainerTagResolver();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryContext(client, null, resolver);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
        }

        #endregion

        #region WithSupermemoryHistory Tests

        [TestMethod]
        public void WithSupermemoryHistory_SetsChatHistoryProviderFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryHistory(client);

            // Assert
            result.Should().BeSameAs(options);
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemoryHistory_WithNullOptions_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            ChatClientAgentOptions? options = null;

            // Act
            var action = () => options!.WithSupermemoryHistory(client);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("options");
        }

        [TestMethod]
        public void WithSupermemoryHistory_WithNullClient_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new ChatClientAgentOptions();

            // Act
            var action = () => options.WithSupermemoryHistory(null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [TestMethod]
        public void WithSupermemoryHistory_WithProviderOptions_SetsFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var providerOptions = new SupermemoryChatHistoryProviderOptions { DefaultContainerTag = "test" };
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryHistory(client, providerOptions);

            // Assert
            result.Should().BeSameAs(options);
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemoryHistory_WithResolver_SetsFactory()
        {
            // Arrange
            using var client = CreateTestClient();
            var resolver = new TestContainerTagResolver();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemoryHistory(client, null, resolver);

            // Assert
            result.Should().BeSameAs(options);
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        #endregion

        #region WithSupermemory Tests

        [TestMethod]
        public void WithSupermemory_SetsBothFactories()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemory(client);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemory_WithNullOptions_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            ChatClientAgentOptions? options = null;

            // Act
            var action = () => options!.WithSupermemory(client);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("options");
        }

        [TestMethod]
        public void WithSupermemory_WithNullClient_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new ChatClientAgentOptions();

            // Act
            var action = () => options.WithSupermemory(null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [TestMethod]
        public void WithSupermemory_WithContextOptions_SetsBothFactories()
        {
            // Arrange
            using var client = CreateTestClient();
            var contextOptions = new SupermemoryContextProviderOptions { DefaultContainerTag = "context" };
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemory(client, contextOptions);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemory_WithHistoryOptions_SetsBothFactories()
        {
            // Arrange
            using var client = CreateTestClient();
            var historyOptions = new SupermemoryChatHistoryProviderOptions { DefaultContainerTag = "history" };
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemory(client, null, historyOptions);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemory_WithResolver_SetsBothFactories()
        {
            // Arrange
            using var client = CreateTestClient();
            var resolver = new TestContainerTagResolver();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemory(client, null, null, resolver);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        [TestMethod]
        public void WithSupermemory_WithAllOptions_SetsBothFactories()
        {
            // Arrange
            using var client = CreateTestClient();
            var contextOptions = new SupermemoryContextProviderOptions { DefaultContainerTag = "context" };
            var historyOptions = new SupermemoryChatHistoryProviderOptions { DefaultContainerTag = "history" };
            var resolver = new TestContainerTagResolver();
            var options = new ChatClientAgentOptions();

            // Act
            var result = options.WithSupermemory(client, contextOptions, historyOptions, resolver);

            // Assert
            result.Should().BeSameAs(options);
            options.AIContextProviderFactory.Should().NotBeNull();
            options.ChatHistoryProviderFactory.Should().NotBeNull();
        }

        #endregion

        #region Helper Methods

        private static SupermemoryClient CreateTestClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.supermemory.ai") };
            var options = Options.Create(new SupermemoryClientOptions { ApiKey = "test-api-key" });
            return new SupermemoryClient(httpClient, options);
        }

        #endregion

        #region Test Helpers

        private class TestContainerTagResolver : IContainerTagResolver
        {
            public string? Resolve(string? template, ContainerTagContext context)
            {
                return "test-resolved";
            }
        }

        #endregion

    }

}
