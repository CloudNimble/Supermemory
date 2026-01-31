using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="DefaultContainerTagResolver"/>.
    /// </summary>
    [TestClass]
    public class DefaultContainerTagResolverTests
    {

        #region Private Members

        private DefaultContainerTagResolver _resolver = null!;

        #endregion

        #region Test Initialization

        [TestInitialize]
        public void Initialize()
        {
            _resolver = new DefaultContainerTagResolver();
        }

        #endregion

        #region Null/Empty Template Tests

        [TestMethod]
        public void Resolve_WithNullTemplate_ReturnsNull()
        {
            // Arrange
            var context = new ContainerTagContext { UserId = "user-123" };

            // Act
            var result = _resolver.Resolve(null, context);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Resolve_WithEmptyTemplate_ReturnsNull()
        {
            // Arrange
            var context = new ContainerTagContext { UserId = "user-123" };

            // Act
            var result = _resolver.Resolve(string.Empty, context);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Placeholder Replacement Tests

        [TestMethod]
        public void Resolve_WithUserIdPlaceholder_ReplacesCorrectly()
        {
            // Arrange
            var context = new ContainerTagContext { UserId = "john-doe" };

            // Act
            var result = _resolver.Resolve("user-{userId}", context);

            // Assert
            result.Should().Be("user-john-doe");
        }

        [TestMethod]
        public void Resolve_WithSessionIdPlaceholder_ReplacesCorrectly()
        {
            // Arrange
            var context = new ContainerTagContext { SessionId = "session-abc123" };

            // Act
            var result = _resolver.Resolve("session-{sessionId}", context);

            // Assert
            result.Should().Be("session-session-abc123");
        }

        [TestMethod]
        public void Resolve_WithTenantIdPlaceholder_ReplacesCorrectly()
        {
            // Arrange
            var context = new ContainerTagContext { TenantId = "acme-corp" };

            // Act
            var result = _resolver.Resolve("tenant-{tenantId}", context);

            // Assert
            result.Should().Be("tenant-acme-corp");
        }

        [TestMethod]
        public void Resolve_WithThreadIdPlaceholder_ReplacesCorrectly()
        {
            // Arrange
            var context = new ContainerTagContext { ThreadId = "thread-xyz" };

            // Act
            var result = _resolver.Resolve("thread-{threadId}", context);

            // Assert
            result.Should().Be("thread-thread-xyz");
        }

        [TestMethod]
        public void Resolve_WithCustomPlaceholder_ReplacesCorrectly()
        {
            // Arrange
            var context = new ContainerTagContext
            {
                CustomValues = new Dictionary<string, object>
                {
                    ["projectId"] = "project-123"
                }
            };

            // Act
            var result = _resolver.Resolve("project-{custom:projectId}", context);

            // Assert
            result.Should().Be("project-project-123");
        }

        #endregion

        #region Missing Context Tests

        [TestMethod]
        public void Resolve_WithMissingUserId_UsesDefault()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            var result = _resolver.Resolve("user-{userId}", context);

            // Assert
            result.Should().Be("user-anonymous");
        }

        [TestMethod]
        public void Resolve_WithMissingSessionId_GeneratesGuid()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            var result = _resolver.Resolve("session-{sessionId}", context);

            // Assert
            result.Should().StartWith("session-");
            result!.Length.Should().BeGreaterThan(8); // Has a GUID appended
        }

        [TestMethod]
        public void Resolve_WithMissingTenantId_UsesDefault()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            var result = _resolver.Resolve("tenant-{tenantId}", context);

            // Assert
            result.Should().Be("tenant-default");
        }

        [TestMethod]
        public void Resolve_WithMissingThreadId_UsesDefault()
        {
            // Arrange
            var context = new ContainerTagContext();

            // Act
            var result = _resolver.Resolve("thread-{threadId}", context);

            // Assert
            result.Should().Be("thread-main");
        }

        [TestMethod]
        public void Resolve_WithMissingCustomValue_KeepsPlaceholder()
        {
            // Arrange
            var context = new ContainerTagContext
            {
                CustomValues = new Dictionary<string, object>()
            };

            // Act
            var result = _resolver.Resolve("value-{custom:missing}", context);

            // Assert
            result.Should().Be("value-{custom:missing}");
        }

        #endregion

        #region Multiple Placeholder Tests

        [TestMethod]
        public void Resolve_WithMultiplePlaceholders_ReplacesAll()
        {
            // Arrange
            var context = new ContainerTagContext
            {
                TenantId = "acme",
                UserId = "john"
            };

            // Act
            var result = _resolver.Resolve("tenant-{tenantId}-user-{userId}", context);

            // Assert
            result.Should().Be("tenant-acme-user-john");
        }

        [TestMethod]
        public void Resolve_WithNoPlaceholders_ReturnsTemplate()
        {
            // Arrange
            var context = new ContainerTagContext { UserId = "john" };

            // Act
            var result = _resolver.Resolve("static-tag", context);

            // Assert
            result.Should().Be("static-tag");
        }

        #endregion

        #region Null Context Test

        [TestMethod]
        public void Resolve_WithNullContext_ThrowsArgumentNullException()
        {
            // Act
            var action = () => _resolver.Resolve("template", null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("context");
        }

        #endregion

    }

}
