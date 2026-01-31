using System.Text.Json;
using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SupermemoryChatHistoryProviderState"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryChatHistoryProviderStateSerializationTests
    {

        #region Serialization Tests

        [TestMethod]
        public void Serialize_ProducesValidJson()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "test-session-key",
                ContainerTag = "user-456",
                MessageCount = 25
            };

            // Act
            var json = JsonSerializer.Serialize(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Assert
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("test-session-key");
            json.Should().Contain("user-456");
        }

        [TestMethod]
        public void Deserialize_RestoresAllProperties()
        {
            // Arrange
            var createdAt = DateTimeOffset.Parse("2024-02-20T14:00:00Z");
            var lastMessageAt = DateTimeOffset.Parse("2024-02-20T15:30:00Z");

            var original = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "session-db-key",
                ContainerTag = "container-tag",
                MessageCount = 15,
                DocumentIds = ["doc-1", "doc-2", "doc-3"],
                CreatedAt = createdAt,
                LastMessageAt = lastMessageAt
            };

            var json = JsonSerializer.Serialize(original, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Act
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Assert
            restored.Should().NotBeNull();
            restored!.SessionDbKey.Should().Be("session-db-key");
            restored.ContainerTag.Should().Be("container-tag");
            restored.MessageCount.Should().Be(15);
            restored.DocumentIds.Should().HaveCount(3);
            restored.CreatedAt.Should().Be(createdAt);
            restored.LastMessageAt.Should().Be(lastMessageAt);
        }

        [TestMethod]
        public void Serialize_WithDocumentIds_IncludesAll()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "test",
                DocumentIds = ["doc-a", "doc-b", "doc-c", "doc-d"]
            };

            // Act
            var json = JsonSerializer.Serialize(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Assert
            json.Should().Contain("doc-a");
            json.Should().Contain("doc-b");
            json.Should().Contain("doc-c");
            json.Should().Contain("doc-d");
        }

        [TestMethod]
        public void SerializeDeserialize_RoundTrip_PreservesState()
        {
            // Arrange
            var original = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "round-trip-key",
                ContainerTag = "round-trip-tag",
                MessageCount = 42,
                DocumentIds = ["x", "y", "z"],
                CreatedAt = DateTimeOffset.UtcNow,
                LastMessageAt = DateTimeOffset.UtcNow
            };

            // Act
            var json = JsonSerializer.Serialize(original, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Assert
            restored.Should().NotBeNull();
            restored!.SessionDbKey.Should().Be(original.SessionDbKey);
            restored.ContainerTag.Should().Be(original.ContainerTag);
            restored.MessageCount.Should().Be(original.MessageCount);
            restored.DocumentIds.Should().BeEquivalentTo(original.DocumentIds);
        }

        #endregion

        #region JsonElement Tests

        [TestMethod]
        public void Serialize_ToJsonElement_ProducesValidElement()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState
            {
                SessionDbKey = "element-key",
                ContainerTag = "element-tag",
                MessageCount = 5
            };

            // Act
            var element = JsonSerializer.SerializeToElement(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryChatHistoryProviderState);

            // Assert
            element.ValueKind.Should().Be(JsonValueKind.Object);
            element.GetProperty("sessionDbKey").GetString().Should().Be("element-key");
            element.GetProperty("containerTag").GetString().Should().Be("element-tag");
            element.GetProperty("messageCount").GetInt32().Should().Be(5);
        }

        #endregion

    }

}
