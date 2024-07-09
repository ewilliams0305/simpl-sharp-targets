using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests;

[Collection(nameof(ClzTestCollection))]
public class ManifestTests
{
    private readonly ClzContainerFixture _fixture;

    public ManifestTests(ClzContainerFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public void ClzCommand_CreatesManifest_WhenTargetIsValid()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var exists = File.Exists(ClzContainerFixture.ManifestPath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }

}