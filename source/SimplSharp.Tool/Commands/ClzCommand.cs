#pragma warning disable IDE0290
#pragma warning disable IDE0041

using Cocona;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SimplSharp.Tool;

/// <summary>
/// Creates a Simpl Sharp Library CLZ archive from a target DLL assembly
/// </summary>
internal sealed class ClzCommand
{
    private readonly ILogger<ClzCommand> _logger;
    private readonly ManifestService _manifestService;
    private readonly ArchiveService _archiveService;
    private readonly ProjectService _projectService;

    /// <summary>
    /// Creates a new instance of the command
    /// </summary>
    /// <param name="logger">A provided console logger</param>
    /// <param name="manifestService">The manifest service to compile a manifest information file.</param>
    /// <param name="archiveService">Service used to pack the CLZ</param>
    public ClzCommand(ILogger<ClzCommand> logger, ManifestService manifestService, ArchiveService archiveService, ProjectService projectService)
    {
        _logger = logger;
        _manifestService = manifestService;
        _archiveService = archiveService;
        _projectService = projectService;
    }


    [Command("clz", Aliases = new string []{"l"}, Description = "Creates a CLZ from the target assembly DLL referencing the SimplSharp SDK")]
    public void CreateClz(
        [Option(
            shortName: 'p',
            Description = "File path for the csproj file that will generate a SimplSharpPro CLZ",
            ValueName = "path")] string path,
        [Option(
            shortName: 't',
            Description = "Target framework to compile",
            ValueName = "target")] string target = "net472",
        [Option(
            shortName: 'a',
            Description = "Output assembly full path, if nothing is provided this argument will default to the bin/",
            ValueName = "assembly")] string? assembly = null,
        [Option(
            shortName: 'c',
            Description = "The target projects build profile",
            ValueName = "profile")] string profile = "Release",
        [Option(
            shortName: 'd',
            Description = "Newly created CLZ destination",
            ValueName = "destination")] string? destination = null)
    {

        var info = new FileInfo(path);

        if (!info.Exists)
        {
            _logger.LogError("Target csproj not found {path}", path);
            Environment.Exit(1);
            return;
        }

        var version = _projectService.QueryProjectForSimplSharpLibraryVersion(path);

        if (version == null)
        {
            _logger.LogError("The target project does not reference the Crestron.SimplSharp.SDK.Library package {path}", path);
            Environment.Exit(2);
            return;
        }

        try
        {
            var assemblyPath = assembly ?? Path.Combine(info.DirectoryName!, "bin", profile, target, info.Name.Replace("csproj", "dll"));
            var loadedAssembly = Assembly.LoadFrom(assemblyPath);

            if (ReferenceEquals(loadedAssembly, null))
            {
                _logger.LogError("Failed to load the assembly at {path}", path);
                Environment.Exit(2);
                return;
            }

            _logger.LogInformation("Creating a SIMPL Sharp Program from the provided assembly {assembly}", loadedAssembly);

            var assemblyName = loadedAssembly.GetName();
            var targetName = assemblyName.Name + Global.LibraryExtension;
            var targetPath = destination is not null
                ? Path.Combine(destination, targetName)
                : assemblyPath.Replace(".dll", Global.LibraryExtension);

            if (!_manifestService.CreateClzManifestXml(
                    assembly: loadedAssembly, 
                    targetName: targetName, 
                    targetPath: targetPath, 
                    sdkVersion: version,
                    out var manifest))
            {
                _logger.LogError("Failed creating the manifest from the assembly {assembly}", assembly);
                Environment.Exit(2);
                return;
            }

            manifest!.Save(_manifestService.CreateManifestFileName(targetPath));

            if (!_archiveService.CreateSimplSharpArchive(TargetType.Library, targetPath, version))
            {
                _logger.LogError("Failed creating the archive from the assembly {assembly}", assembly);
                Environment.Exit(2);
            }

            _logger.LogInformation("Successfully created {extension} from {assembly} located at {targetPath}", Global.LibraryExtension, assembly, targetPath);

            Environment.Exit(0);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Target project not found {path}", path);
            Environment.Exit(100);
        }
    }
}
