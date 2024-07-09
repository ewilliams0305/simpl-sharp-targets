using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests;

[Collection(nameof(ClzTestCollection))]
public class ArchiveTests
{
    private readonly ClzContainerFixture _fixture;

    public ArchiveTests(ClzContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ClzCommand_ExitsClean_WhenArchive_IsCreated()
    {
        var (output, exitCode) = _fixture.CreateClz();

        exitCode.Should().Be(0, because: "The command should complete with a valid file");
    }
    
    [Fact]
    public void ClzCommand_ExitsBad_WhenArchive_IsNotCreated()
    {
        var (output, exitCode) = _fixture.CreateClz("invalid_file.dll");

        exitCode.Should().Be(-1, because: "The command should not create an clz from nothing");
    }
    
    [Fact]
    public void ClzCommand_CreatesArchiveFile_WhenAssemblyIsValidTarget()
    {
        _fixture.DeleteExistingClz();

        var (output, exitCode) = _fixture.CreateClz();

        var exists = File.Exists(ClzContainerFixture.TargetArchivePath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }
    
    [Fact]
    public void ClzCommand_DoesNotCreatesArchiveFile_WhenAssemblyIsNotValidTarget()
    {
        _fixture.DeleteExistingClz();

        var (output, exitCode) = _fixture.CreateClz("invalid_file.dll");

        var exists = File.Exists(ClzContainerFixture.TargetArchivePath);

        exists.Should().BeFalse(because: "The assembly path provided is not a valid DLL");
    }
}