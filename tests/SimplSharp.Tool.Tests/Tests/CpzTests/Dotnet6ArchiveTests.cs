using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests.CpzTests;

[Collection(nameof(ClzTestCollection))]
public class Dotnet6ArchiveTests(ExecutableTestFixture fixture)
{

    [Fact]
    public void ClzCommand_ExitsClean_WhenArchive_IsCreated()
    {
        var (_, exitCode) = fixture.CreateNet6Cpz();

        exitCode.Should().Be(0, because: "The command should complete with a valid file");
    }
    
    [Fact]
    public void ClzCommand_ExitsBad_WhenArchive_IsNotCreated()
    {
        var (_, exitCode) = fixture.CreateNet6Cpz("invalid_file.csproj");

        exitCode.Should().Be(1, because: "The command should not create an clz from nothing");
    }
    
    [Fact]
    public void ClzCommand_CreatesArchiveFile_WhenProjectIsValidTarget()
    {

        FilePaths.DeleteExistingNet6Cpz();

        fixture.CreateNet6Cpz();

        var exists = File.Exists(FilePaths.TargetProgramNet6ArchivePath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }
    
    [Fact]
    public void ClzCommand_DoesNotCreatesArchiveFile_WhenProjectIsNotValidTarget()
    {
        FilePaths.DeleteExistingNet6Cpz();

        fixture.CreateNet6Cpz("invalid_file.csproj");

        var exists = File.Exists(FilePaths.TargetProgramNet6ArchivePath);

        exists.Should().BeFalse(because: "The assembly path provided is not a valid DLL");
    }
}