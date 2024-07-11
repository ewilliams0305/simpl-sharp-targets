using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests;

[Collection(nameof(ClzTestCollection))]
public class ArchiveTests(ClzTestFixture fixture)
{

    [Fact]
    public void ClzCommand_ExitsClean_WhenArchive_IsCreated()
    {
        var (_, exitCode) = fixture.CreateClz();

        exitCode.Should().Be(0, because: "The command should complete with a valid file");
    }
    
    [Fact]
    public void ClzCommand_ExitsBad_WhenArchive_IsNotCreated()
    {
        var (_, exitCode) = fixture.CreateClz("invalid_file.dll");

        exitCode.Should().NotBe(0, because: "The command should not create an clz from nothing");
    }
    
    [Fact]
    public void ClzCommand_CreatesArchiveFile_WhenAssemblyIsValidTarget()
    {
        FilePaths.DeleteExistingClz();

        fixture.CreateClz();

        var exists = File.Exists(FilePaths.TargetArchivePath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }
    
    [Fact]
    public void ClzCommand_DoesNotCreatesArchiveFile_WhenAssemblyIsNotValidTarget()
    {
        FilePaths.DeleteExistingClz();

        fixture.CreateClz("invalid_file.dll");

        var exists = File.Exists(FilePaths.TargetArchivePath);

        exists.Should().BeFalse(because: "The assembly path provided is not a valid DLL");
    }
}