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
    /// Tests for <see cref="SupermemoryContextProvider"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryContextProviderTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_WithClient_InitializesState()
        {
            // Arrange
            using var client = CreateTestClient();

            // Act
            var provider = new SupermemoryContextProvider(client);

            // Assert
            provider.State.Should().NotBeNull();
            provider.State.SessionId.Should().NotBeNullOrEmpty();
            provider.State.StoredMemoryIds.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithOptions_AppliesOptions()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                SearchLimit = 10
            };

            // Act
            var provider = new SupermemoryContextProvider(client, options);

            // Assert
            provider.ContainerTag.Should().Be("test-container");
        }

        [TestMethod]
        public void Constructor_WithNullClient_ThrowsArgumentNullException()
        {
            // Act
            var action = () => new SupermemoryContextProvider(null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [TestMethod]
        public void Constructor_WithSerializedState_RestoresState()
        {
            // Arrange
            using var client = CreateTestClient();
            var state = new SupermemoryContextProviderState
            {
                SessionId = "restored-session",
                ContainerTag = "restored-container",
                TurnNumber = 5
            };
            var serializedState = JsonSerializer.SerializeToElement(
                state,
                SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Act
            var provider = new SupermemoryContextProvider(client, serializedState);

            // Assert
            provider.State.SessionId.Should().Be("restored-session");
            provider.State.ContainerTag.Should().Be("restored-container");
            provider.State.TurnNumber.Should().Be(5);
        }

        [TestMethod]
        public void Constructor_WithEmptySerializedState_UsesDefaults()
        {
            // Arrange
            using var client = CreateTestClient();
            var emptyState = default(JsonElement);

            // Act
            var provider = new SupermemoryContextProvider(client, emptyState);

            // Assert
            provider.State.Should().NotBeNull();
            provider.State.SessionId.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void Constructor_WithResolver_UsesCustomResolver()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "user-{userId}"
            };
            var resolver = new TestContainerTagResolver("resolved-tag");

            // Act
            var provider = new SupermemoryContextProvider(client, options, resolver);

            // Assert
            provider.ContainerTag.Should().Be("resolved-tag");
        }

        #endregion

        #region ContainerTag Property Tests

        [TestMethod]
        public void ContainerTag_Get_ReturnsStateValue()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);
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
            var provider = new SupermemoryContextProvider(client);

            // Act
            provider.ContainerTag = "new-container-tag";

            // Assert
            provider.State.ContainerTag.Should().Be("new-container-tag");
        }

        [TestMethod]
        public void ContainerTag_SetNull_ClearsValue()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);
            provider.ContainerTag = "initial-tag";

            // Act
            provider.ContainerTag = null;

            // Assert
            provider.ContainerTag.Should().BeNull();
        }

        #endregion

        #region Serialize Tests

        [TestMethod]
        public void Serialize_ReturnsValidJson()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);
            provider.ContainerTag = "serialization-test";

            // Act
            var result = provider.Serialize();

            // Assert
            result.ValueKind.Should().Be(JsonValueKind.Object);
        }

        [TestMethod]
        public void Serialize_IncludesAllStateProperties()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);
            provider.State.ContainerTag = "test-container";
            provider.State.TurnNumber = 3;

            // Act
            var result = provider.Serialize();

            // Assert
            result.TryGetProperty("containerTag", out var containerTag).Should().BeTrue();
            containerTag.GetString().Should().Be("test-container");
            result.TryGetProperty("turnNumber", out var turnNumber).Should().BeTrue();
            turnNumber.GetInt32().Should().Be(3);
        }

        [TestMethod]
        public void Serialize_RoundTrip_PreservesState()
        {
            // Arrange
            using var client = CreateTestClient();
            var originalProvider = new SupermemoryContextProvider(client);
            originalProvider.ContainerTag = "round-trip-container";
            originalProvider.State.TurnNumber = 7;
            originalProvider.State.StoredMemoryIds.Add("mem-1");

            // Act
            var serialized = originalProvider.Serialize();
            var restoredProvider = new SupermemoryContextProvider(client, serialized);

            // Assert
            restoredProvider.ContainerTag.Should().Be("round-trip-container");
            restoredProvider.State.TurnNumber.Should().Be(7);
            restoredProvider.State.StoredMemoryIds.Should().Contain("mem-1");
        }

        #endregion

        #region InvokingAsync Tests

        [TestMethod]
        public async Task InvokingAsync_WithNoContainerTag_ReturnsEmptyContext()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);
            // ContainerTag is null by default

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Instructions.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public async Task InvokingAsync_WithEmptyQueryText_ReturnsEmptyContext()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions { DefaultContainerTag = "test-container" };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokingContext([]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Instructions.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public async Task InvokingAsync_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);

            // Act
            var action = () => provider.InvokingAsync(null!).AsTask();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task InvokingAsync_WithApiError_ReturnsEmptyContext()
        {
            // Arrange
            // Use a client that will fail when making API calls (no valid endpoint)
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                RetrievalStrategy = MemoryRetrievalStrategy.ProfileFirst
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Instructions.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public async Task InvokingAsync_WithSearchOnlyStrategy_ReturnsEmptyContextOnError()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                RetrievalStrategy = MemoryRetrievalStrategy.SearchOnly
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task InvokingAsync_WithProfileOnlyStrategy_ReturnsEmptyContextOnError()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                RetrievalStrategy = MemoryRetrievalStrategy.ProfileOnly
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task InvokingAsync_WithBothStrategy_ReturnsEmptyContextOnError()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                RetrievalStrategy = MemoryRetrievalStrategy.Both
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokingContext(["Hello"]);

            // Act
            var result = await provider.InvokingAsync(context);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region InvokedAsync Tests

        [TestMethod]
        public async Task InvokedAsync_WithException_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions { DefaultContainerTag = "test" };
            var provider = new SupermemoryContextProvider(client, options);
            var initialTurnNumber = provider.State.TurnNumber;

            var context = CreateInvokedContext(["Hello"], ["Hi there!"], new InvalidOperationException("Test"));

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.TurnNumber.Should().Be(initialTurnNumber);
        }

        [TestMethod]
        public async Task InvokedAsync_WithStorageDisabled_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test",
                EnableConversationStorage = false
            };
            var provider = new SupermemoryContextProvider(client, options);
            var initialTurnNumber = provider.State.TurnNumber;

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.TurnNumber.Should().Be(initialTurnNumber);
        }

        [TestMethod]
        public async Task InvokedAsync_WithNoContainerTag_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions { EnableConversationStorage = true };
            var provider = new SupermemoryContextProvider(client, options);
            var initialTurnNumber = provider.State.TurnNumber;

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.TurnNumber.Should().Be(initialTurnNumber);
        }

        [TestMethod]
        public async Task InvokedAsync_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);

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
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act
            var action = () => provider.InvokedAsync(context).AsTask();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task InvokedAsync_WithEmptyMessages_ReturnsEarly()
        {
            // Arrange
            using var client = CreateTestClient();
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test",
                EnableConversationStorage = true
            };
            var provider = new SupermemoryContextProvider(client, options);
            var initialTurnNumber = provider.State.TurnNumber;

            var context = CreateInvokedContext([], []);

            // Act
            await provider.InvokedAsync(context);

            // Assert
            provider.State.TurnNumber.Should().Be(initialTurnNumber);
        }

        [TestMethod]
        public async Task InvokedAsync_WithMarkdownFormat_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true,
                StorageFormat = ConversationStorageFormat.Markdown
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act & Assert
            await provider.InvokedAsync(context);
        }

        [TestMethod]
        public async Task InvokedAsync_WithPlainTextFormat_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true,
                StorageFormat = ConversationStorageFormat.PlainText
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act & Assert
            await provider.InvokedAsync(context);
        }

        [TestMethod]
        public async Task InvokedAsync_WithJsonFormat_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true,
                StorageFormat = ConversationStorageFormat.Json
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act & Assert
            await provider.InvokedAsync(context);
        }

        [TestMethod]
        public async Task InvokedAsync_WithUserMessagesOnly_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true,
                StoreUserMessagesOnly = true
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act & Assert
            await provider.InvokedAsync(context);
        }

        [TestMethod]
        public async Task InvokedAsync_WithDefaultMetadata_DoesNotThrow()
        {
            // Arrange
            using var client = CreateTestClientWithTimeout(TimeSpan.FromMilliseconds(100));
            var options = new SupermemoryContextProviderOptions
            {
                DefaultContainerTag = "test-container",
                EnableConversationStorage = true,
                DefaultMetadata = new Dictionary<string, object> { ["app"] = "test" }
            };
            var provider = new SupermemoryContextProvider(client, options);

            var context = CreateInvokedContext(["Hello"], ["Hi there!"]);

            // Act & Assert
            await provider.InvokedAsync(context);
        }

        #endregion

        #region GetService Tests

        [TestMethod]
        public void GetService_SupermemoryClient_ReturnsClient()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);

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
            var options = new SupermemoryContextProviderOptions { SearchLimit = 20 };
            var provider = new SupermemoryContextProvider(client, options);

            // Act
            var result = provider.GetService<SupermemoryContextProviderOptions>();

            // Assert
            result.Should().NotBeNull();
            result!.SearchLimit.Should().Be(20);
        }

        [TestMethod]
        public void GetService_State_ReturnsState()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);

            // Act
            var result = provider.GetService<SupermemoryContextProviderState>();

            // Assert
            result.Should().BeSameAs(provider.State);
        }

        [TestMethod]
        public void GetService_UnknownType_ReturnsNull()
        {
            // Arrange
            using var client = CreateTestClient();
            var provider = new SupermemoryContextProvider(client);

            // Act
            var result = provider.GetService<string>();

            // Assert
            result.Should().BeNull();
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

        private static AIContextProvider.InvokingContext CreateInvokingContext(string[] messages)
        {
            var chatMessages = messages.Select(m => new ChatMessage(ChatRole.User, m)).ToList();
            return new AIContextProvider.InvokingContext(chatMessages);
        }

        private static AIContextProvider.InvokedContext CreateInvokedContext(string[] requestMessages, string[] responseMessages, Exception? exception = null)
        {
            var requests = requestMessages.Select(m => new ChatMessage(ChatRole.User, m)).ToList();
            var responses = responseMessages.Select(m => new ChatMessage(ChatRole.Assistant, m)).ToList();
            return new AIContextProvider.InvokedContext(requests, responses)
            {
                InvokeException = exception
            };
        }

        #endregion

        #region Test Helpers

        private class TestContainerTagResolver : IContainerTagResolver
        {
            private readonly string _result;

            public TestContainerTagResolver(string result)
            {
                _result = result;
            }

            public string? Resolve(string? template, ContainerTagContext context)
            {
                return _result;
            }
        }

        #endregion

    }

}
