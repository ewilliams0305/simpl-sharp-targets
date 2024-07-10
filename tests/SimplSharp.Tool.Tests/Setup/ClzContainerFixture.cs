﻿#pragma warning disable CA1822

using System.Diagnostics;
using System.Xml.Linq;

namespace SimplSharp.Tool.Tests.Setup;

/// <summary>
/// Runs the tool as a dotnet process.
/// Since this test project depends on the other projects they should build first.
/// Once built the tests target the output of the tooling as well as the library project.
/// </summary>
public class ClzContainerFixture : IAsyncLifetime
{

    public string ReferencedSdkVersion { get; set; } = string.Empty;

#if DEBUG
    public static readonly string ExePath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Tool", "bin", "Debug", "net8.0", "SimplSharp.Tool.exe");
    public static readonly string TargetLibraryPath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Debug", "net472", "SimplSharp.Library.dll");
    public static readonly string TargetLibraryProject = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "SimplSharp.Library.csproj");
    public static readonly string TargetArchivePath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Debug", "net472", "SimplSharp.Library.clz");
    public static readonly string ManifestPath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Debug", "net472", "ProgramInfo.config");
#else
    public static readonly string ExePath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Tool", "bin", "Release", "net8.0", "SimplSharp.Tool.exe");
    public static readonly string TargetLibraryPath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Release", "net472", "SimplSharp.Library.dll");
    public static readonly string TargetLibraryProject = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "SimplSharp.Library.csproj");
    public static readonly string TargetArchivePath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Release", "net472", "SimplSharp.Library.clz");
    public static readonly string ManifestPath = Path.Combine("..", "..", "..", "..", "..", "source", "SimplSharp.Library", "bin", "Release", "net472", "ProgramInfo.config");
#endif

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        var current = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory("../../../../../");

        var newDir = Directory.GetCurrentDirectory();

        Console.WriteLine($"-------------> current: {current}");
        Console.WriteLine($"-------------> updated: {newDir}");

        if (!File.Exists(ExePath))
        {
            throw new FileNotFoundException(ExePath);
        }

        if (!File.Exists(TargetLibraryPath))
        {
            throw new FileNotFoundException(TargetLibraryPath);
        }
        
        if (!File.Exists(TargetLibraryProject))
        {
            throw new FileNotFoundException(TargetLibraryProject);
        }

        var xdoc = XDocument.Load(TargetLibraryProject);

        ReferencedSdkVersion = xdoc.Descendants("PackageReference")
            .Where(x => (string)x.Attribute("Include")! == "Crestron.SimplSharp.SDK.Library")
            .Select(x => (string)x.Attribute("Version")!)
            .FirstOrDefault()!;

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