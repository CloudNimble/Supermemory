using System.Text.Json;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using FluentAssertions;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="SupermemoryChatHistoryProvider"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryChatHistoryProviderTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithClient_InitializesState()
        {
            // Arrange
            using var client = CreateTestClient();

            // Act
            var provider = new SupermemoryChatHistoryProvider(client);

            // Assert
            provider.State.Should().NotBeNull();
            provider.SessionDbKey.Should().NotBeNullOrEmpty();
            provider.State.DocumentIds.Should().NotBeNull();
            provider.State.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        }

        [TestMethod]
        public void Constructor_WithOptions_AppliesOptions()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "history-container",
                MaxMessages = 100
            };

            // Act
            var provider = new SupermemoryChatHistoryProvider(client, options);

            // Assert
            provider.ContainerTag.Should().Be("history-container");
        }

        [TestMethod]
        public void Constructor_WithNullClient_ThrowsArgumentNullException()
        {
            // Act
            var action = () => new SupermemoryChatHistoryProvider(null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [TestMethod]
        public void Constructor_WithSerializedState_RestoresSessionDbKey()
        {
            // Arrange
            using var client = CreateTestClient();
            var state = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "restored-db-key",
                ContainerTag = "restored-container",
                MessageCount = 15,
                DocumentIds = ["doc-1", "doc-2"]
            };
            var serializedState = JsonSerializer.SerializeToElement(
                state,
                SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Act
            var provider = new SupermemoryChatHistoryProvider(client, serializedState);

            // Assert
            provider.SessionDbKey.Should().Be("restored-db-key");
            provider.ContainerTag.Should().Be("restored-container");
            provider.State.MessageCount.Should().Be(15);
            provider.State.DocumentIds.Should().HaveCount(2);
        }

        [TestMethod]
        public void Constructor_GeneratesNewSessionDbKey()
        {
            // Arrange
            using var client = CreateTestClient();

            // Act
            var provider1 = new SupermemoryChatHistoryProvider(client);
            var provider2 = new SupermemoryChatHistoryProvider(client);

            // Assert
            provider1.SessionDbKey.Should().NotBe(provider2.SessionDbKey);
        }

        #endregion

        #region ContainerTag Property Tests

        [TestMethod]
        public void ContainerTag_Get_ReturnsStateValue()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);
            provider.State.ContainerTag = "direct-state-tag";

            // Act
            var result = provider.ContainerTag;

            // Assert
            result.Should().Be("direct-state-tag");
        }

        [TestMethod]
        public void ContainerTag_Set_UpdatesStateValue()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            provider.ContainerTag = "new-container-tag";

            // Assert
            provider.State.ContainerTag.Should().Be("new-container-tag");
        }

        #endregion

        #region Serialize Tests

        [TestMethod]
        public void Serialize_IncludesSessionDbKey()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var result = provider.Serialize();

            // Assert
            result.ValueKind.Should().Be(JsonValueKind.Object);
            result.TryGetProperty("sessionDbKey", out var sessionKey).Should().BeTrue();
            sessionKey.GetString().Should().Be(provider.SessionDbKey);
        }

        [TestMethod]
        public void Serialize_RoundTrip_PreservesState()
        {
            // Arrange
            using var client = CreateTestClient();
            var originalProvider = new SupermemoryChatHistoryProvider(client);
            originalProvider.ContainerTag = "round-trip-container";
            originalProvider.State.MessageCount = 42;
            originalProvider.State.DocumentIds.Add("doc-x");

            // Act
            var serialized = originalProvider.Serialize();
            var restoredProvider = new SupermemoryChatHistoryProvider(client, serialized);

            // Assert
            restoredProvider.SessionDbKey.Should().Be(originalProvider.SessionDbKey);
            restoredProvider.ContainerTag.Should().Be("round-trip-container");
            restoredProvider.State.MessageCount.Should().Be(42);
            restoredProvider.State.DocumentIds.Should().Contain("doc-x");
        }

        #endregion

        #region InvokingAsync Tests

        [TestMethod]
        public async Task InvokingAsync_WithNoContainerTag_ReturnsEmpty()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);
            // ContainerTag is null by default

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task InvokingAsync_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var action = () => provider.InvokingAsync(null!).AsTask();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task InvokingAsync_WithApiError_ReturnsEmpty()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "test-container"
            };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region InvokedAsync Tests

        [TestMethod]
        public async Task InvokedAsync_WithException_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryChatHistoryProviderOptions { DefaultContainerTag = "test" };
            var provider = new SupermemoryChatHistoryProvider(client, options);
            var initialMessageCount = provider.State.MessageCount;

            var context = CreateInvokedContext(["Hello"], ["Hi there!"], new InvalidOperationException("Test"));

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.MessageCount.Should().Be(initialMessageCount);
        }

        [TestMethod]
        public async Task InvokedAsync_WithNoContainerTag_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);
            var initialMessageCount = provider.State.MessageCount;

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.MessageCount.Should().Be(initialMessageCount);
        }

        [TestMethod]
        public async Task InvokedAsync_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var action = () => provider.InvokedAsync(null!).AsTask();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task InvokedAsync_WithApiError_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "test-container"
            };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            var action = () => provider.InvokedAsync(context).AsTask();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task InvokedAsync_WithEmptyMessages_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "test-container"
            };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            var context = CreateInvokedContext([], []);

            // Act
            var action = () => provider.InvokedAsync(context).AsTask();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task InvokedAsync_WithContextProviderMessagesEnabled_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "test-container",
                StoreContextProviderMessages = true
            };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            var action = () => provider.InvokedAsync(context).AsTask();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task InvokedAsync_WithDefaultMetadata_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryChatHistoryProviderOptions
            {
                DefaultContainerTag = "test-container",
                DefaultMetadata = new Dictionary<string, object> { ["app"] = "test" }
            };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            var action = () => provider.InvokedAsync(context).AsTask();

            // Assert
            await action.Should().NotThrowAsync();
        }

        #endregion

        #region GetService Tests

        [TestMethod]
        public void GetService_SupermemoryClient_ReturnsClient()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var result = provider.GetService<SupermemoryClient>();

            // Assert
            result.Should().BeSameAs(client);
        }

        [TestMethod]
        public void GetService_Options_ReturnsOptions()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryChatHistoryProviderOptions { MaxMessages = 200 };
            var provider = new SupermemoryChatHistoryProvider(client, options);

            // Act
            var result = provider.GetService<SupermemoryChatHistoryProviderOptions>();

            // Assert
            result.Should().NotBeNull();
            result!.MaxMessages.Should().Be(200);
        }

        [TestMethod]
        public void GetService_State_ReturnsState()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var result = provider.GetService<SupermemoryChatHistoryProviderState>();

            // Assert
            result.Should().BeSameAs(provider.State);
        }

        [TestMethod]
        public void GetService_UnknownType_ReturnsNull()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryChatHistoryProvider(client);

            // Act
            var result = provider.GetService<int>();

            // Assert
            result.Should().Be(default);
        }

        #endregion

        #region Helper Methods

        private static SupermemoryClient CreateTestClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.supermemory.ai") };
            var options = Options.Create(new SupermemoryClientOptions { ApiKey = "test-api-key" });
            return new SupermemoryClient(httpClient, options);
        }

        private static SupermemoryClient CreateTestClientWithTimeout(TimeSpan timeout)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:1/"),
                Timeout = timeout
            };
            var options = Options.Create(new SupermemoryClientOptions
            {
                ApiKey = "test-api-key",
                Timeout = timeout
            });
            return new SupermemoryClient(httpClient, options);
        }

        private static ChatHistoryProvider.InvokingContext CreateInvokingContext(string[] messages)
        {
            var chatMessages = messages.Select(m => new ChatMessage(ChatRole.User, m)).ToList();
            return new ChatHistoryProvider.InvokingContext(chatMessages);
        }

        private static ChatHistoryProvider.InvokedContext CreateInvokedContext(string[] requestMessages, string[] responseMessages, Exception? exception = null)
        {
            var requests = requestMessages.Select(m => new ChatMessage(ChatRole.User, m)).ToList();
            var responses = responseMessages.Select(m => new ChatMessage(ChatRole.Assistant, m)).ToList();
            return new ChatHistoryProvider.InvokedContext(requests, responses)
            {
                InvokeException = exception
            };
        }

        #endregion

    }

}
