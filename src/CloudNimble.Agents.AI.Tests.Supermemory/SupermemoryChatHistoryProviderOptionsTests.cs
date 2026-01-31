using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="SupermemoryChatHistoryProviderOptions"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryChatHistoryProviderOptionsTests
    {

        #region Default Value Tests

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            // Act
            var options = new SupermemoryChatHistoryProviderOptions();

            // Assert
            options.DefaultContainerTag.Should().BeNull();
            options.MaxMessages.Should().Be(50);
            options.ChatReducer.Should().BeNull();
            options.ReducerTrigger.Should().Be(ChatReducerTriggerEvent.BeforeMessagesRetrieval);
            options.DocumentIdPrefix.Should().Be("chat-");
            options.DefaultMetadata.Should().BeNull();
            options.StoreContextProviderMessages.Should().BeFalse();
        }

        #endregion

        #region Property Setter Tests

        [TestMethod]
        public void DefaultContainerTag_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();

            // Act
            options.DefaultContainerTag = "session-{sessionId}";

            // Assert
            options.DefaultContainerTag.Should().Be("session-{sessionId}");
        }

        [TestMethod]
        public void MaxMessages_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();

            // Act
            options.MaxMessages = 100;

            // Assert
            options.MaxMessages.Should().Be(100);
        }

        [TestMethod]
        public void DocumentIdPrefix_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();

            // Act
            options.DocumentIdPrefix = "msg-";

            // Assert
            options.DocumentIdPrefix.Should().Be("msg-");
        }

        [TestMethod]
        public void StoreContextProviderMessages_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();

            // Act
            options.StoreContextProviderMessages = true;

            // Assert
            options.StoreContextProviderMessages.Should().BeTrue();
        }

        [TestMethod]
        public void ReducerTrigger_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();

            // Act
            options.ReducerTrigger = ChatReducerTriggerEvent.AfterMessageAdded;

            // Assert
            options.ReducerTrigger.Should().Be(ChatReducerTriggerEvent.AfterMessageAdded);
        }

        [TestMethod]
        public void DefaultMetadata_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryChatHistoryProviderOptions();
            var metadata = new Dictionary<string, object> { ["tenant"] = "acme" };

            // Act
            options.DefaultMetadata = metadata;

            // Assert
            options.DefaultMetadata.Should().ContainKey("tenant");
        }

        #endregion

    }

}
