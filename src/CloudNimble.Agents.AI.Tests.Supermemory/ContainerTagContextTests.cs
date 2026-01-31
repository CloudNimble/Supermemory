using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="ContainerTagContext"/>.
    /// </summary>
    [TestClass]
    public class ContainerTagContextTests
    {

        #region Default Value Tests

        [TestMethod]
        public void DefaultValues_AreNull()
        {
            // Act
            var context = new ContainerTagContext();

            // Assert
            context.UserId.Should().BeNull();
            context.SessionId.Should().BeNull();
            context.TenantId.Should().BeNull();
            context.ThreadId.Should().BeNull();
            context.CustomValues.Should().BeNull();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void UserId_CanBeSet()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            context.UserId = "user-123";

            // Assert
            context.UserId.Should().Be("user-123");
        }

        [TestMethod]
        public void SessionId_CanBeSet()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            context.SessionId = "session-456";

            // Assert
            context.SessionId.Should().Be("session-456");
        }

        [TestMethod]
        public void TenantId_CanBeSet()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            context.TenantId = "tenant-789";

            // Assert
            context.TenantId.Should().Be("tenant-789");
        }

        [TestMethod]
        public void ThreadId_CanBeSet()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            context.ThreadId = "thread-abc";

            // Assert
            context.ThreadId.Should().Be("thread-abc");
        }

        [TestMethod]
        public void CustomValues_CanBeSet()
        {
            // Arrange
            var context = new ContainerTagContext();
            var customValues = new Dictionary<string, object>
            {
                ["key1"] = "value1",
                ["key2"] = 123
            };

            // Act
            context.CustomValues = customValues;

            // Assert
            context.CustomValues.Should().HaveCount(2);
            context.CustomValues!["key1"].Should().Be("value1");
            context.CustomValues["key2"].Should().Be(123);
        }

        #endregion

        #region Object Initializer Tests

        [TestMethod]
        public void ObjectInitializer_SetsAllProperties()
        {
            // Act
            var context = new ContainerTagContext
            {
                UserId = "user",
                SessionId = "session",
                TenantId = "tenant",
                ThreadId = "thread",
                CustomValues = new Dictionary<string, object> { ["key"] = "value" }
            };

            // Assert
            context.UserId.Should().Be("user");
            context.SessionId.Should().Be("session");
            context.TenantId.Should().Be("tenant");
            context.ThreadId.Should().Be("thread");
            context.CustomValues.Should().ContainKey("key");
        }

        #endregion

    }

}
