using System.Runtime.InteropServices;
using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests.ClzTests;

[Collection(nameof(ClzTestCollection))]
public class WrapperTests(ExecutableTestFixture fixture)
{

    [Fact]
    public void SimplPlusCompiler_ExitsClean_WhenWrapperCompiles()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            true.Should().Be(true, because: "We cant compile a SIMPL+ wrapper on linux or a machine without the Crestron compilers installed");
            return;
        }

        FilePaths.DeleteWrapperClz();
        FilePaths.DeleteExistingUsh();

        fixture.CreateClz(FilePaths.WrapperLibraryPath);

        var (output, exitCode) = fixture.CompileSimplPlus();

        exitCode.Should().Be(0, because: "The compiler should succeed as there is a valid CLZ");
    }

    [Fact]
    public void SimplPlusCompiler_ExitsFailed_WhenWrapperFailsCompilation()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            true.Should().Be(true, because: "We cant compile a SIMPL+ wrapper on linux or a machine without the Crestron compilers installed");
            return;
        }

        FilePaths.DeleteWrapperClz();
        FilePaths.DeleteExistingUsh();

        var (_, exitCode) = fixture.CompileSimplPlus();

        exitCode.Should().NotBe(0, because: "The compiler should fail as the CLZ has been deleted");
    }

    [Fact]
    public void SimplPlusCompiler_CreateUshFile_WhenWrapperCompiles()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            true.Should().Be(true, because: "We cant compile a SIMPL+ wrapper on linux or a machine without the Crestron compilers installed");
            return;
        }

        FilePaths.DeleteWrapperClz();
        FilePaths.DeleteExistingUsh();

        fixture.CreateClz(FilePaths.WrapperLibraryPath);

        fixture.CompileSimplPlus();

        
        File.Exists(FilePaths.UserPlusCompiledPath).Should().BeTrue(because: "The compiler should has created a .ush file");
    }
    
    [Fact(Skip = "Need to sort this out")]
    public void SimplPlusCompiler_DoesNotCreateUshFile_WhenCompilerFails()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            true.Should().Be(true, because: "We cant compile a SIMPL+ wrapper on linux or a machine without the Crestron compilers installed");
            return;
        }

        FilePaths.DeleteExistingSpls();
        FilePaths.DeleteWrapperClz();
        FilePaths.DeleteExistingUsh();

        var (output, exitCode) = fixture.CompileSimplPlus();

        var exists = File.Exists(FilePaths.UserPlusCompiledPath);

        exists.Should().BeFalse(because: "There there is no CLZ so the SIMPL compiler can't compile");
    }

}