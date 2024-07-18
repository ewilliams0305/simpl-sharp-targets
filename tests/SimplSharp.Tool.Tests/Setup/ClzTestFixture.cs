#pragma warning disable CA1822

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Crestron.SimplSharp.CrestronIO;
using File = System.IO.File;

namespace SimplSharp.Tool.Tests.Setup;

/// <summary>
/// Runs the tool as a dotnet process.
/// Since this test project depends on the other projects they should build first.
/// Once built the tests target the output of the tooling as well as the library project.
/// </summary>
public class ClzTestFixture : IAsyncLifetime
{

    public string ReferencedSdkVersion { get; set; } = string.Empty;

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        var xdoc = XDocument.Load(FilePaths.TargetLibraryProject);

        ReferencedSdkVersion = xdoc.Descendants("PackageReference")
            .Where(x => (string)x.Attribute("Include")! == "Crestron.SimplSharp.SDK.Library")
            .Select(x => (string)x.Attribute("Version")!)
            .FirstOrDefault()!;

        return Task.CompletedTask;
    }


    public (string output, int exitCode) CreateClz(string? targetProject = null, string? destination = null)
    {
        var projectPath = targetProject ?? FilePaths.TargetLibraryProject;
#if DEBUG
        var args = destination != null
            ? "clz -p " + projectPath + " -c Debug" + "-d" + destination
            : "clz -p " + projectPath + " -c Debug";
#else
        var args = destination != null
            ? "clz -p " + projectPath + " -c Release" + "-d" + destination
            : "clz -p " + projectPath + " -c Release";
#endif

        var processStartInfo = new ProcessStartInfo
        {
            FileName = FilePaths.ExePath,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = args,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return (output, process.ExitCode);
    }

    public (string output, int exitCode) CompileSimplPlus(string? targetWrapper = null)
    {
        if (!File.Exists(@"C:\Program Files (x86)\Crestron\Simpl\SPlusCC.exe"))
        {
            return ("", 1);
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = @"C:\Program Files (x86)\Crestron\Simpl\SPlusCC.exe",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = targetWrapper == null
                ? @$"""\rebuild"" ""{FilePaths.UserPlusWrapperPath}"" ""\target"" ""series4"""
                : @$"""\rebuild"" ""{targetWrapper}"" ""\target"" ""series4""",
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return (output, process.ExitCode);
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        FilePaths.DeleteExistingClz();
        FilePaths.DeleteExistingManifest();
        FilePaths.DeleteExistingLpz();
        FilePaths.DeleteExistingUsh();
        FilePaths.DeleteExistingSpls();

        return Task.CompletedTask;
    }
}