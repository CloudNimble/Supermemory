using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="SupermemoryChatHistoryProviderState"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryChatHistoryProviderStateTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_GeneratesSessionDbKey()
        {
            // Act
            var state = new SupermemoryChatHistoryProviderState();

            // Assert
            state.SessionDbKey.Should().NotBeNullOrEmpty();
            state.SessionDbKey.Should().HaveLength(32); // GUID without dashes
        }

        [TestMethod]
        public void Constructor_InitializesCreatedAt()
        {
            // Act
            var before = DateTimeOffset.UtcNow;
            var state = new SupermemoryChatHistoryProviderState();
            var after = DateTimeOffset.UtcNow;

            // Assert
            state.CreatedAt.Should().BeOnOrAfter(before);
            state.CreatedAt.Should().BeOnOrBefore(after);
        }

        [TestMethod]
        public void Constructor_InitializesEmptyDocumentIds()
        {
            // Act
            var state = new SupermemoryChatHistoryProviderState();

            // Assert
            state.DocumentIds.Should().NotBeNull();
            state.DocumentIds.Should().BeEmpty();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void SessionDbKey_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState();
            var customKey = "custom-session-key";

            // Act
            state.SessionDbKey = customKey;

            // Assert
            state.SessionDbKey.Should().Be(customKey);
        }

        [TestMethod]
        public void ContainerTag_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState();

            // Act
            state.ContainerTag = "user-456";

            // Assert
            state.ContainerTag.Should().Be("user-456");
        }

        [TestMethod]
        public void MessageCount_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState();

            // Act
            state.MessageCount = 10;

            // Assert
            state.MessageCount.Should().Be(10);
        }

        [TestMethod]
        public void DocumentIds_CanBeModified()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState();

            // Act
            state.DocumentIds.Add("doc-1");
            state.DocumentIds.Add("doc-2");

            // Assert
            state.DocumentIds.Should().HaveCount(2);
            state.DocumentIds.Should().Contain("doc-1");
            state.DocumentIds.Should().Contain("doc-2");
        }

        [TestMethod]
        public void LastMessageAt_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryChatHistoryProviderState();
            var timestamp = DateTimeOffset.UtcNow;

            // Act
            state.LastMessageAt = timestamp;

            // Assert
            state.LastMessageAt.Should().Be(timestamp);
        }

        #endregion

    }

}
