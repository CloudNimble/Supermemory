using System.Net.Http;
using CloudNimble.Agents.AI.Supermemory;
using CloudNimble.Supermemory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory.Extensions
{

    /// <summary>
    /// Tests for Supermemory ServiceCollection extensions.
    /// These tests are excluded until API call mocking is implemented.
    /// </summary>
    [TestClass]
    [Ignore("Tests excluded until API call mocking is implemented")]
    public class ServiceCollectionExtensionsTests
    {

        #region AddSupermemoryContextProvider Tests

        [TestMethod]
        public void AddSupermemoryContextProvider_RegistersServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            var result = services.AddSupermemoryContextProvider();

            // Assert
            result.Should().BeSameAs(services);

            var provider = services.BuildServiceProvider();
            var resolver = provider.GetService<IContainerTagResolver>();
            resolver.Should().BeOfType<DefaultContainerTagResolver>();
        }

        [TestMethod]
        public void AddSupermemoryContextProvider_WithNullServices_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act
            var action = () => services!.AddSupermemoryContextProvider();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [TestMethod]
        public void AddSupermemoryContextProvider_WithConfiguration_AppliesConfig()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            services.AddSupermemoryContextProvider(options =>
            {
                options.DefaultContainerTag = "configured-tag";
                options.SearchLimit = 15;
            });

            // Assert
            var provider = services.BuildServiceProvider();
            var contextProvider = provider.GetService<SupermemoryContextProvider>();
            contextProvider.Should().NotBeNull();
        }

        [TestMethod]
        public void AddSupermemoryContextProvider_WithoutConfiguration_UsesDefaults()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            services.AddSupermemoryContextProvider();

            // Assert
            var provider = services.BuildServiceProvider();
            var contextProvider = provider.GetService<SupermemoryContextProvider>();
            contextProvider.Should().NotBeNull();
            contextProvider!.ContainerTag.Should().BeNull(); // Default is null
        }

        [TestMethod]
        public void AddSupermemoryContextProvider_ResolvesProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());
            services.AddSupermemoryContextProvider(options =>
            {
                options.DefaultContainerTag = "resolved-tag";
            });

            // Act
            var provider = services.BuildServiceProvider();
            var contextProvider = provider.GetService<SupermemoryContextProvider>();

            // Assert
            contextProvider.Should().NotBeNull();
            contextProvider!.ContainerTag.Should().Be("resolved-tag");
        }

        #endregion

        #region AddSupermemoryChatHistoryProvider Tests

        [TestMethod]
        public void AddSupermemoryChatHistoryProvider_RegistersServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            var result = services.AddSupermemoryChatHistoryProvider();

            // Assert
            result.Should().BeSameAs(services);

            var provider = services.BuildServiceProvider();
            var resolver = provider.GetService<IContainerTagResolver>();
            resolver.Should().BeOfType<DefaultContainerTagResolver>();
        }

        [TestMethod]
        public void AddSupermemoryChatHistoryProvider_WithNullServices_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act
            var action = () => services!.AddSupermemoryChatHistoryProvider();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [TestMethod]
        public void AddSupermemoryChatHistoryProvider_WithConfiguration_AppliesConfig()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            services.AddSupermemoryChatHistoryProvider(options =>
            {
                options.DefaultContainerTag = "history-configured-tag";
                options.MaxMessages = 150;
            });

            // Assert
            var provider = services.BuildServiceProvider();
            var historyProvider = provider.GetService<SupermemoryChatHistoryProvider>();
            historyProvider.Should().NotBeNull();
        }

        [TestMethod]
        public void AddSupermemoryChatHistoryProvider_WithoutConfiguration_UsesDefaults()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            services.AddSupermemoryChatHistoryProvider();

            // Assert
            var provider = services.BuildServiceProvider();
            var historyProvider = provider.GetService<SupermemoryChatHistoryProvider>();
            historyProvider.Should().NotBeNull();
            historyProvider!.ContainerTag.Should().BeNull(); // Default is null
        }

        [TestMethod]
        public void AddSupermemoryChatHistoryProvider_ResolvesProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());
            services.AddSupermemoryChatHistoryProvider(options =>
            {
                options.DefaultContainerTag = "resolved-history-tag";
            });

            // Act
            var provider = services.BuildServiceProvider();
            var historyProvider = provider.GetService<SupermemoryChatHistoryProvider>();

            // Assert
            historyProvider.Should().NotBeNull();
            historyProvider!.ContainerTag.Should().Be("resolved-history-tag");
        }

        #endregion

        #region AddSupermemoryAgentFramework Tests

        [TestMethod]
        public void AddSupermemoryAgentFramework_RegistersBothProviders()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            var result = services.AddSupermemoryAgentFramework();

            // Assert
            result.Should().BeSameAs(services);

            var provider = services.BuildServiceProvider();
            var contextProvider = provider.GetService<SupermemoryContextProvider>();
            var historyProvider = provider.GetService<SupermemoryChatHistoryProvider>();

            contextProvider.Should().NotBeNull();
            historyProvider.Should().NotBeNull();
        }

        [TestMethod]
        public void AddSupermemoryAgentFramework_WithNullServices_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act
            var action = () => services!.AddSupermemoryAgentFramework();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [TestMethod]
        public void AddSupermemoryAgentFramework_WithConfiguration_AppliesBothConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());

            // Act
            services.AddSupermemoryAgentFramework(
                contextOptions => contextOptions.DefaultContainerTag = "context-configured",
                historyOptions => historyOptions.DefaultContainerTag = "history-configured");

            // Assert
            var provider = services.BuildServiceProvider();
            var contextProvider = provider.GetService<SupermemoryContextProvider>();
            var historyProvider = provider.GetService<SupermemoryChatHistoryProvider>();

            contextProvider!.ContainerTag.Should().Be("context-configured");
            historyProvider!.ContainerTag.Should().Be("history-configured");
        }

        #endregion

        #region AddContainerTagResolver Tests

        [TestMethod]
        public void AddContainerTagResolver_RegistersCustomResolver()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddContainerTagResolver<TestContainerTagResolver>();

            // Assert
            result.Should().BeSameAs(services);

            var provider = services.BuildServiceProvider();
            var resolver = provider.GetService<IContainerTagResolver>();
            resolver.Should().BeOfType<TestContainerTagResolver>();
        }

        [TestMethod]
        public void AddContainerTagResolver_WithNullServices_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act
            var action = () => services!.AddContainerTagResolver<TestContainerTagResolver>();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [TestMethod]
        public void AddContainerTagResolver_OverridesDefaultResolver()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(_ => CreateTestClient());
            services.AddSupermemoryContextProvider();

            // Act
            services.AddContainerTagResolver<TestContainerTagResolver>();

            // Assert
            var provider = services.BuildServiceProvider();
            var resolver = provider.GetService<IContainerTagResolver>();
            resolver.Should().BeOfType<TestContainerTagResolver>();
        }

        #endregion

        #region Helper Methods

        private static SupermemoryClient CreateTestClient()
        {
            return new SupermemoryClient(new SupermemoryClientOptions
            {
                HttpClient = new HttpClient()
            });
        }

        #endregion

        #region Test Helpers

        private class TestContainerTagResolver : IContainerTagResolver
        {
            public string? Resolve(string? template, ContainerTagContext context)
            {
                return "test-resolved";
            }
        }

        #endregion

    }

}
