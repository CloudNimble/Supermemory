using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="SupermemoryContextProviderState"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryContextProviderStateTests
    {

        #region Constructor Tests

        [TestMethod]
        public void Constructor_GeneratesSessionId()
        {
            // Act
            var state = new SupermemoryContextProviderState();

            // Assert
            state.SessionId.Should().NotBeNullOrEmpty();
            state.SessionId.Should().HaveLength(32); // GUID without dashes
        }

        [TestMethod]
        public void Constructor_InitializesEmptyLists()
        {
            // Act
            var state = new SupermemoryContextProviderState();

            // Assert
            state.StoredMemoryIds.Should().NotBeNull();
            state.StoredMemoryIds.Should().BeEmpty();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void SessionId_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();
            var customId = "custom-session-id";

            // Act
            state.SessionId = customId;

            // Assert
            state.SessionId.Should().Be(customId);
        }

        [TestMethod]
        public void ContainerTag_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();

            // Act
            state.ContainerTag = "user-123";

            // Assert
            state.ContainerTag.Should().Be("user-123");
        }

        [TestMethod]
        public void StoredMemoryIds_CanBeModified()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();

            // Act
            state.StoredMemoryIds.Add("memory-1");
            state.StoredMemoryIds.Add("memory-2");

            // Assert
            state.StoredMemoryIds.Should().HaveCount(2);
            state.StoredMemoryIds.Should().Contain("memory-1");
            state.StoredMemoryIds.Should().Contain("memory-2");
        }

        [TestMethod]
        public void LastProfileFetch_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();
            var timestamp = DateTimeOffset.UtcNow;

            // Act
            state.LastProfileFetch = timestamp;

            // Assert
            state.LastProfileFetch.Should().Be(timestamp);
        }

        [TestMethod]
        public void TurnNumber_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();

            // Act
            state.TurnNumber = 5;

            // Assert
            state.TurnNumber.Should().Be(5);
        }

        [TestMethod]
        public void CustomData_CanBeSet()
        {
            // Arrange
            var state = new SupermemoryContextProviderState();
            var customData = new Dictionary<string, object> { ["key"] = "value" };

            // Act
            state.CustomData = customData;

            // Assert
            state.CustomData.Should().ContainKey("key");
        }

        #endregion

    }

}
