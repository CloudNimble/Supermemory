using System.Text.Json;
using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="SupermemoryContextProviderState"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryContextProviderStateSerializationTests
    {

        #region Serialization Tests

        [TestMethod]
        public void Serialize_ProducesValidJson()
        {
            // Arrange
            var state = new SupermemoryContextProviderState
            {
                SessionId = "test-session-id",
                ContainerTag = "user-123",
                TurnNumber = 5
            };

            // Act
            var json = JsonSerializer.Serialize(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Assert
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("test-session-id");
            json.Should().Contain("user-123");
        }

        [TestMethod]
        public void Deserialize_RestoresAllProperties()
        {
            // Arrange
            var original = new SupermemoryContextProviderState
            {
                SessionId = "session-abc",
                ContainerTag = "container-xyz",
                TurnNumber = 3,
                LastProfileFetch = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
                StoredMemoryIds = ["mem-1", "mem-2"],
                CustomData = new Dictionary<string, object> { ["key"] = "value" }
            };

            var json = JsonSerializer.Serialize(original, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Act
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Assert
            restored.Should().NotBeNull();
            restored!.SessionId.Should().Be("session-abc");
            restored.ContainerTag.Should().Be("container-xyz");
            restored.TurnNumber.Should().Be(3);
            restored.StoredMemoryIds.Should().HaveCount(2);
        }

        [TestMethod]
        public void Serialize_WithNullProperties_OmitsNulls()
        {
            // Arrange
            var state = new SupermemoryContextProviderState
            {
                SessionId = "test",
                ContainerTag = null,
                CustomData = null
            };

            // Act
            var json = JsonSerializer.Serialize(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Assert
            json.Should().NotContain("\"containerTag\":");
            json.Should().NotContain("\"customData\":");
        }

        [TestMethod]
        public void SerializeDeserialize_RoundTrip_PreservesState()
        {
            // Arrange
            var original = new SupermemoryContextProviderState
            {
                SessionId = "round-trip-session",
                ContainerTag = "round-trip-container",
                TurnNumber = 10,
                StoredMemoryIds = ["id1", "id2", "id3"]
            };

            // Act
            var json = JsonSerializer.Serialize(original, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);
            var restored = JsonSerializer.Deserialize(json, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Assert
            restored.Should().NotBeNull();
            restored!.SessionId.Should().Be(original.SessionId);
            restored.ContainerTag.Should().Be(original.ContainerTag);
            restored.TurnNumber.Should().Be(original.TurnNumber);
            restored.StoredMemoryIds.Should().BeEquivalentTo(original.StoredMemoryIds);
        }

        #endregion

        #region JsonElement Tests

        [TestMethod]
        public void Serialize_ToJsonElement_ProducesValidElement()
        {
            // Arrange
            var state = new SupermemoryContextProviderState
            {
                SessionId = "element-test",
                ContainerTag = "element-container"
            };

            // Act
            var element = JsonSerializer.SerializeToElement(state, SupermemoryAgentFrameworkJsonContext.Default.SupermemoryContextProviderState);

            // Assert
            element.ValueKind.Should().Be(JsonValueKind.Object);
            element.GetProperty("sessionId").GetString().Should().Be("element-test");
            element.GetProperty("containerTag").GetString().Should().Be("element-container");
        }

        #endregion

    }

}
