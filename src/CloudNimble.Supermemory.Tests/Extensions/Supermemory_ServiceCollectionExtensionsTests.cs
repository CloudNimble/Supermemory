using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Extensions
{

    /// <summary>
    /// Tests for <see cref="Supermemory_ServiceCollectionExtensions"/>.
    /// </summary>
    [TestClass]
    public class Supermemory_ServiceCollectionExtensionsTests
    {

        #region AddSupermemory Tests

        [TestMethod]
        public void AddSupermemory_RegistersOptionsFromConfiguration()
        {
            // Arrange
            var configValues = new Dictionary<string, string?>
            {
                ["Supermemory:ApiKey"] = "test-api-key",
                ["Supermemory:BaseUrl"] = "https://custom.api.com",
                ["Supermemory:Timeout"] = "00:02:00",
                ["Supermemory:MaxRetries"] = "5"
            };

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(configValues);
            builder.Services.AddSupermemory();

            using var host = builder.Build();

            // Act
            var options = host.Services.GetRequiredService<IOptions<SupermemoryClientOptions>>();

            // Assert
            options.Value.ApiKey.Should().Be("test-api-key");
            options.Value.BaseUrl.Should().Be("https://custom.api.com");
            options.Value.Timeout.Should().Be(TimeSpan.FromMinutes(2));
            options.Value.MaxRetries.Should().Be(5);
        }

        [TestMethod]
        public void AddSupermemory_RegistersSupermemoryClient()
        {
            // Arrange
            var configValues = new Dictionary<string, string?>
            {
                ["Supermemory:ApiKey"] = "test-api-key"
            };

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(configValues);
            builder.Services.AddSupermemory();

            using var host = builder.Build();

            // Act
            var client = host.Services.GetRequiredService<SupermemoryClient>();

            // Assert
            client.Should().NotBeNull();
            client.Documents.Should().NotBeNull();
            client.Search.Should().NotBeNull();
            client.Memories.Should().NotBeNull();
            client.Connections.Should().NotBeNull();
            client.Settings.Should().NotBeNull();
            client.Profile.Should().NotBeNull();
        }

        [TestMethod]
        public void AddSupermemory_WithCustomSection_BindsFromThatSection()
        {
            // Arrange
            var configValues = new Dictionary<string, string?>
            {
                ["CustomSection:ApiKey"] = "custom-api-key",
                ["CustomSection:BaseUrl"] = "https://custom.example.com"
            };

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(configValues);
            builder.Services.AddSupermemory("CustomSection", _ => { });

            using var host = builder.Build();

            // Act
            var options = host.Services.GetRequiredService<IOptions<SupermemoryClientOptions>>();

            // Assert
            options.Value.ApiKey.Should().Be("custom-api-key");
            options.Value.BaseUrl.Should().Be("https://custom.example.com");
        }

        [TestMethod]
        public void AddSupermemory_WithConfigure_OverridesConfigurationValues()
        {
            // Arrange
            var configValues = new Dictionary<string, string?>
            {
                ["Supermemory:ApiKey"] = "config-api-key",
                ["Supermemory:Timeout"] = "00:01:00"
            };

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(configValues);
            builder.Services.AddSupermemory("Supermemory", options =>
            {
                options.Timeout = TimeSpan.FromSeconds(120);
                options.MaxRetries = 10;
            });

            using var host = builder.Build();

            // Act
            var options = host.Services.GetRequiredService<IOptions<SupermemoryClientOptions>>();

            // Assert
            options.Value.ApiKey.Should().Be("config-api-key");
            options.Value.Timeout.Should().Be(TimeSpan.FromSeconds(120));
            options.Value.MaxRetries.Should().Be(10);
        }

        [TestMethod]
        public void AddSupermemory_WithNullServices_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection services = null!;

            // Act
            var action = () => services.AddSupermemory();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [TestMethod]
        public void AddSupermemory_WithNullSectionName_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var action = () => services.AddSupermemory(sectionName: null!, configure: _ => { });

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithParameterName("sectionName");
        }

        [TestMethod]
        public void AddSupermemory_WithEmptySectionName_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var action = () => services.AddSupermemory(sectionName: "", configure: _ => { });

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithParameterName("sectionName");
        }

        [TestMethod]
        public void AddSupermemory_WithNullConfigure_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var action = () => services.AddSupermemory(configure: null!);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("configure");
        }

        [TestMethod]
        public void AddSupermemory_UsesDefaultValues_WhenNotConfigured()
        {
            // Arrange
            var configValues = new Dictionary<string, string?>
            {
                ["Supermemory:ApiKey"] = "test-api-key"
            };

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(configValues);
            builder.Services.AddSupermemory();

            using var host = builder.Build();

            // Act
            var options = host.Services.GetRequiredService<IOptions<SupermemoryClientOptions>>();

            // Assert
            options.Value.BaseUrl.Should().Be(SupermemoryClientOptions.DefaultBaseUrl);
            options.Value.Timeout.Should().Be(TimeSpan.FromSeconds(SupermemoryClientOptions.DefaultTimeoutSeconds));
            options.Value.MaxRetries.Should().Be(SupermemoryClientOptions.DefaultMaxRetries);
        }

        #endregion

    }

}
