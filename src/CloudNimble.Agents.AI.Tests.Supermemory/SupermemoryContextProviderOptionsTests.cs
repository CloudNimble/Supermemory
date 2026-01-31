using CloudNimble.Agents.AI.Supermemory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Agents.AI.Tests.Supermemory
{

    /// <summary>
    /// Tests for <see cref="SupermemoryContextProviderOptions"/>.
    /// </summary>
    [TestClass]
    public class SupermemoryContextProviderOptionsTests
    {

        #region Default Value Tests

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            // Act
            var options = new SupermemoryContextProviderOptions();

            // Assert
            options.DefaultContainerTag.Should().BeNull();
            options.RetrievalStrategy.Should().Be(MemoryRetrievalStrategy.ProfileFirst);
            options.UseProfileApi.Should().BeTrue();
            options.UseSearchApi.Should().BeTrue();
            options.SearchMode.Should().Be(CloudNimble.Supermemory.SearchMode.Memories);
            options.SearchLimit.Should().Be(5);
            options.MinimumSimilarityScore.Should().Be(0.7);
            options.RerankResults.Should().BeFalse();
            options.ProfileThreshold.Should().Be(0.7);
            options.EnableConversationStorage.Should().BeTrue();
            options.StorageFormat.Should().Be(ConversationStorageFormat.Markdown);
            options.StoreUserMessagesOnly.Should().BeFalse();
            options.InstructionTemplate.Should().Contain("{staticMemories}");
            options.InstructionTemplate.Should().Contain("{dynamicMemories}");
            options.InstructionTemplate.Should().Contain("{searchMemories}");
            options.DefaultMetadata.Should().BeNull();
        }

        #endregion

        #region Property Setter Tests

        [TestMethod]
        public void DefaultContainerTag_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.DefaultContainerTag = "user-{userId}";

            // Assert
            options.DefaultContainerTag.Should().Be("user-{userId}");
        }

        [TestMethod]
        public void RetrievalStrategy_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.RetrievalStrategy = MemoryRetrievalStrategy.Both;

            // Assert
            options.RetrievalStrategy.Should().Be(MemoryRetrievalStrategy.Both);
        }

        [TestMethod]
        public void SearchLimit_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.SearchLimit = 10;

            // Assert
            options.SearchLimit.Should().Be(10);
        }

        [TestMethod]
        public void MinimumSimilarityScore_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.MinimumSimilarityScore = 0.9;

            // Assert
            options.MinimumSimilarityScore.Should().Be(0.9);
        }

        [TestMethod]
        public void InstructionTemplate_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();
            var template = "Custom template: {staticMemories}";

            // Act
            options.InstructionTemplate = template;

            // Assert
            options.InstructionTemplate.Should().Be(template);
        }

        [TestMethod]
        public void EnableConversationStorage_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.EnableConversationStorage = false;

            // Assert
            options.EnableConversationStorage.Should().BeFalse();
        }

        [TestMethod]
        public void StorageFormat_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.StorageFormat = ConversationStorageFormat.Json;

            // Assert
            options.StorageFormat.Should().Be(ConversationStorageFormat.Json);
        }

        [TestMethod]
        public void StoreUserMessagesOnly_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();

            // Act
            options.StoreUserMessagesOnly = true;

            // Assert
            options.StoreUserMessagesOnly.Should().BeTrue();
        }

        [TestMethod]
        public void DefaultMetadata_CanBeSet()
        {
            // Arrange
            var options = new SupermemoryContextProviderOptions();
            var metadata = new Dictionary<string, object> { ["key"] = "value" };

            // Act
            options.DefaultMetadata = metadata;

            // Assert
            options.DefaultMetadata.Should().ContainKey("key");
            options.DefaultMetadata!["key"].Should().Be("value");
        }

        #endregion

    }

}
