using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="ChatMessageWrapper"/>.
    /// </summary>
    [TestClass]
    public class ChatMessageWrapperTests
    {

        #region Default Value Tests

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            // Act
            var wrapper = new ChatMessageWrapper();

            // Assert
            wrapper.Id.Should().Be(string.Empty);
            wrapper.Role.Should().Be(string.Empty);
            wrapper.Content.Should().Be(string.Empty);
            wrapper.Timestamp.Should().Be(default);
            wrapper.Metadata.Should().BeNull();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void Id_CanBeSet()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper();

            // Act
            wrapper.Id = "msg-123";

            // Assert
            wrapper.Id.Should().Be("msg-123");
        }

        [TestMethod]
        public void Role_CanBeSet()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper();

            // Act
            wrapper.Role = "user";

            // Assert
            wrapper.Role.Should().Be("user");
        }

        [TestMethod]
        public void Content_CanBeSet()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper();

            // Act
            wrapper.Content = "Hello, world!";

            // Assert
            wrapper.Content.Should().Be("Hello, world!");
        }

        [TestMethod]
        public void Timestamp_CanBeSet()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper();
            var timestamp = DateTimeOffset.UtcNow;

            // Act
            wrapper.Timestamp = timestamp;

            // Assert
            wrapper.Timestamp.Should().Be(timestamp);
        }

        [TestMethod]
        public void Metadata_CanBeSet()
        {
            // Arrange
            var wrapper = new ChatMessageWrapper();
            var metadata = new Dictionary<string, object> { ["key"] = "value" };

            // Act
            wrapper.Metadata = metadata;

            // Assert
            wrapper.Metadata.Should().ContainKey("key");
            wrapper.Metadata!["key"].Should().Be("value");
        }

        #endregion

        #region Object Initializer Tests

        [TestMethod]
        public void ObjectInitializer_SetsAllProperties()
        {
            // Arrange
            var timestamp = DateTimeOffset.UtcNow;

            // Act
            var wrapper = new ChatMessageWrapper
            {
                Id = "msg-456",
                Role = "assistant",
                Content = "How can I help?",
                Timestamp = timestamp,
                Metadata = new Dictionary<string, object> { ["source"] = "agent" }
            };

            // Assert
            wrapper.Id.Should().Be("msg-456");
            wrapper.Role.Should().Be("assistant");
            wrapper.Content.Should().Be("How can I help?");
            wrapper.Timestamp.Should().Be(timestamp);
            wrapper.Metadata.Should().ContainKey("source");
        }

        #endregion

    }

}
