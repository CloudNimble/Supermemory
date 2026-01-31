using System.Text.Json;
using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ChatMessageWrapper"/>.
    /// </summary>
    [TestClass]
    public class ChatMessageWrapperSerializationTests
    {

        #region Serialization Tests

        [TestMethod]
        public void Serialize_ProducesValidJson()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper
            {
                Id = "msg-001",
                Role = "user",
                Content = "Hello there!",
                Timestamp = DateTimeOffset.Parse("2024-03-01T12:00:00Z")
            };

            // Act
            var json = JsonSerializer.Serialize(wrapper, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

            // Assert
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("msg-001");
            json.Should().Contain("user");
            json.Should().Contain("Hello there!");
        }

        [TestMethod]
        public void Deserialize_RestoresAllProperties()
        {
            // Arrange
            var timestamp = DateTimeOffset.Parse("2024-03-01T12:00:00Z");
            var original = new ChatMessageWrapper
            {
                Id = "msg-002",
                Role = "assistant",
                Content = "How can I assist you?",
                Timestamp = timestamp,
                Metadata = new Dictionary<string, object> { ["model"] = "gpt-4" }
            };

            var json = JsonSerializer.Serialize(original, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

            // Act
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

            // Assert
            restored.Should().NotBeNull();
            restored!.Id.Should().Be("msg-002");
            restored.Role.Should().Be("assistant");
            restored.Content.Should().Be("How can I assist you?");
            restored.Timestamp.Should().Be(timestamp);
        }

        [TestMethod]
        public void SerializeDeserialize_List_RoundTrip()
        {
            // Arrange
            var messages = new List<ChatMessageWrapper>
            {
                new()
                {
                    Id = "1",
                    Role = "user",
                    Content = "First message",
                    Timestamp = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = "2",
                    Role = "assistant",
                    Content = "Second message",
                    Timestamp = DateTimeOffset.UtcNow
                }
            };

            // Act
            var json = JsonSerializer.Serialize(messages, SupermemoryAgentFrameworkJsonContext.Default.ListChatMessageWrapper);
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.ListChatMessageWrapper);

            // Assert
            restored.Should().NotBeNull();
            restored.Should().HaveCount(2);
            restored![0].Id.Should().Be("1");
            restored[1].Id.Should().Be("2");
        }

        #endregion

        #region Edge Cases

        [TestMethod]
        public void Serialize_WithSpecialCharacters_EncodesCorrectly()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper
            {
                Id = "special",
                Role = "user",
                Content = "Test with \"quotes\" and \\ backslash and 日本語",
                Timestamp = DateTimeOffset.UtcNow
            };

            // Act
            var json = JsonSerializer.Serialize(wrapper, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

            // Assert
            restored.Should().NotBeNull();
            restored!.Content.Should().Be("Test with \"quotes\" and \\ backslash and 日本語");
        }

        [TestMethod]
        public void Serialize_WithEmptyContent_Works()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper
            {
                Id = "empty",
                Role = "system",
                Content = string.Empty,
                Timestamp = DateTimeOffset.UtcNow
            };

            // Act
            var json = JsonSerializer.Serialize(wrapper, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.ChatMessageWrapper);

            // Assert
            restored.Should().NotBeNull();
            restored!.Content.Should().BeEmpty();
        }

        #endregion

    }

}
