using System.Text.Json;
using CloudNimble.Supermemory.Models.Profile;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.Supermemory.Tests.Serialization
{

    /// <summary>
    /// Serialization tests for <see cref="ProfileResponse"/> using API sample payloads.
    /// </summary>
    [TestClass]
    public class ProfileResponseTests
    {

        [TestMethod]
        [Description("POST /v4/profile - 200 response sample from API docs")]
        public void Deserialize_200Response_ReturnsValidObject()
        {
            var json = """
                {
                    "profile": {
                        "dynamic": [
                            "Recently interested in machine learning",
                            "Working on a Python project"
                        ],
                        "static": [
                            "Prefers dark mode",
                            "Uses VS Code as primary editor"
                        ]
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ProfileResponse>(json, SupermemoryJsonContext.Default.ProfileResponse);

            response.Should().NotBeNull();
            response!.Profile.Should().NotBeNull();
            response.Profile.Dynamic.Should().HaveCount(2);
            response.Profile.Static.Should().HaveCount(2);
            response.Profile.Dynamic.Should().Contain("Recently interested in machine learning");
            response.Profile.Static.Should().Contain("Prefers dark mode");
        }

        [TestMethod]
        [Description("POST /v4/profile - Response with search results")]
        public void Deserialize_WithSearchResults_ReturnsValidObject()
        {
            var json = """
                {
                    "profile": {
                        "dynamic": ["Current focus: API development"],
                        "static": ["Experience: 5 years"]
                    },
                    "searchResults": {
                        "results": [],
                        "timing": 25.5,
                        "total": 0
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ProfileResponse>(json, SupermemoryJsonContext.Default.ProfileResponse);

            response.Should().NotBeNull();
            response!.SearchResults.Should().NotBeNull();
            response.SearchResults!.Timing.Should().Be(25.5);
            response.SearchResults.Total.Should().Be(0);
        }

        [TestMethod]
        [Description("POST /v4/profile - Empty profile")]
        public void Deserialize_EmptyProfile_ReturnsValidObject()
        {
            var json = """
                {
                    "profile": {
                        "dynamic": [],
                        "static": []
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ProfileResponse>(json, SupermemoryJsonContext.Default.ProfileResponse);

            response.Should().NotBeNull();
            response!.Profile.Dynamic.Should().BeEmpty();
            response.Profile.Static.Should().BeEmpty();
        }

        [TestMethod]
        [Description("POST /v4/profile - Rich profile with search")]
        public void Deserialize_RichProfileWithSearch_ReturnsValidObject()
        {
            var json = """
                {
                    "profile": {
                        "dynamic": [
                            "Learning Rust programming",
                            "Building a CLI tool",
                            "Interested in WebAssembly"
                        ],
                        "static": [
                            "Senior software engineer",
                            "Background in systems programming",
                            "Open source contributor"
                        ]
                    },
                    "searchResults": {
                        "results": [{}],
                        "timing": 45.2,
                        "total": 1
                    }
                }
                """;

            var response = JsonSerializer.Deserialize<ProfileResponse>(json, SupermemoryJsonContext.Default.ProfileResponse);

            response.Should().NotBeNull();
            response!.Profile.Dynamic.Should().HaveCount(3);
            response.Profile.Static.Should().HaveCount(3);
            response.SearchResults!.Total.Should().Be(1);
        }

    }

}
