using System.Diagnostics;

namespace SimplSharp.Tool.Tests.Setup;

/// <summary>
/// Runs the tool as a dotnet process.
/// Since this test project depends on the other projects they should build first.
/// Once built the tests target the output of the tooling as well as the library project.
/// </summary>
public class ClzContainerFixture : IAsyncLifetime
{
#if DEBUG
    public const string ExePath = @"../../../../../source/SimplSharp.Tool/bin/Debug/net8.0/SimplSharp.Tool.exe";
    public const string TargetLibraryPath = @"../../../../../source/SimplSharp.Library/bin/Debug/net472/SimplSharp.Library.dll";
    public const string TargetArchivePath = @"../../../../../source/SimplSharp.Library/bin/Debug/net472/SimplSharp.Library.clz";
    public const string ManifestPath = @"../../../../../source/SimplSharp.Library/bin/Debug/net472/ProgramInfo.config";
#else
    public const string ExePath = @"../../../../../source/SimplSharp.Tool/bin/Release/net8.0/SimplSharp.Tool.exe";
    public const string TargetLibraryPath = @"../../../../../source/SimplSharp.Library/bin/Release/net472/SimplSharp.Library.dll";
    public const string TargetArchivePath = @"../../../../../source/SimplSharp.Library/bin/Release/net472/SimplSharp.Library.clz";
    public const string ManifestPath = @"../../../../../source/SimplSharp.Library/bin/Release/net472/ProgramInfo.config";
#endif

    /// <inheritdoc />
    public Task InitializeAsync()
    {

        if (!File.Exists(ExePath))
        {
            throw new FileNotFoundException(ExePath);
        }

        if (!File.Exists(TargetLibraryPath))
        {
            throw new FileNotFoundException(TargetLibraryPath);
        }

        return Task.CompletedTask;
    }

    public void DeleteExistingClz()
    {
        if (File.Exists(TargetArchivePath))
        {
            File.Delete(TargetArchivePath);
        }
    }
    public void DeleteExistingManifest()
    {
        if (File.Exists(ManifestPath))
        {
            File.Delete(ManifestPath);
        }
    }

    public (string output, int exitCode) CreateClz(string? targetAssembly = null, string? destination = null)
    {
        var assemblyPath = targetAssembly ?? TargetLibraryPath;
        var args = destination != null
            ? "clz -p " + assemblyPath + "-d" + destination
            : "clz -p " + assemblyPath;

        var processStartInfo = new ProcessStartInfo
        {
            FileName = ExePath,
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

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        DeleteExistingClz();
        DeleteExistingManifest();

        return Task.CompletedTask;
    }
}