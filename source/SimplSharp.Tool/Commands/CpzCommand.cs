#pragma warning disable IDE0290
#pragma warning disable IDE0300

using Cocona;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SimplSharp.Tool;

/// <summary>
/// Creates a Simpl Sharp Library CLZ archive from a target DLL assembly
/// </summary>
internal sealed class CpzCommand
{
    private readonly ILogger<CpzCommand> _logger;
    private readonly ManifestService _manifestService;
    private readonly ArchiveService _archiveService;
    private readonly ProjectService _projectService;

    /// <summary>
    /// Creates a new instance of the command
    /// </summary>
    /// <param name="logger">A provided console logger</param>
    /// <param name="manifestService">The manifest service to compile a manifest information file.</param>
    /// <param name="archiveService">Service used to pack the CPZ</param>
    public CpzCommand(ILogger<CpzCommand> logger, ManifestService manifestService, ArchiveService archiveService, ProjectService projectService)
    {
        _logger = logger;
        _manifestService = manifestService;
        _archiveService = archiveService;
        _projectService = projectService;
    }


    [Command("cpz", Aliases = new string []{"p"}, Description = "Creates a CPZ from the target assembly DLL referencing the SimplSharpPro SDK")]
    public void CreateCpz(
        [Option(
            shortName: 'p',
            Description = "File path for the csproj file that will generate a SimplSharpPro CPZ",
            ValueName = "path")] string path,
        [Option(
            shortName: 't',
            Description = "Target framework to compile",
            ValueName = "target")] string target,
        [Option(
            shortName: 'a',
            Description = "Output assembly full path, if nothing is provided this argument will default to the bin/",
            ValueName = "assembly")] string? assembly,
        [Option(
            shortName: 'c',
            Description = "The target projects build profile",
            ValueName = "profile")] string profile = "Release",
        [Option(
            shortName: 'd',
            Description = "Newly created CPZ destination",
            ValueName = "destination")] string? destination = null)
    {
        var info = new FileInfo(path);

        if (!info.Exists)
        {
            _logger.LogError("Target csproj was not found {path}", path);
            Environment.Exit(1);
            return;
        }

        var version = _projectService.QueryProjectForSimplSharpProgramVersion(path);

        if (version == null)
        {
            _logger.LogError("The target project does not reference the Crestron.SimplSharp.SDK.Program package {path}", path);
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
            var targetName = assemblyName.Name + Global.ProgramExtension;
            var targetPath = destination is not null
            ? Path.Combine(destination, targetName)
            : assemblyPath.Replace(".dll", Global.ProgramExtension);

            if (target == "net472")
            {
                TargetDotnetFramework(assembly, loadedAssembly, targetName, targetPath, version);
                return;
            }

            TargetDotnetCore(assembly, loadedAssembly, targetName, targetPath, version);

        }

        catch (Exception e)
        {
            _logger.LogError(e, "Target project not found {path}", path);
            Environment.Exit(100);
        }
    }

    private void TargetDotnetFramework(string? assembly, Assembly loadedAssembly, string targetName, string targetPath, Version version)
    {
        if (!_manifestService.CreateCpzManifestXml(
                assembly: loadedAssembly,
                targetName: targetName,
                targetPath: targetPath,
                version,
                out var manifest))
        {
            _logger.LogError("Failed creating the manifest from the assembly {assembly}", assembly);
            Environment.Exit(2);
        }

        manifest!.Save(_manifestService.CreateManifestFileName(targetPath));

        if (!_archiveService.CreateSimplSharpArchive(TargetType.Library, targetPath, version))
        {
            _logger.LogError("Failed creating the archive from the assembly {assembly}", assembly);
            Environment.Exit(2);
        }

        _logger.LogInformation("Successfully created {extension} from {assembly} located at {targetPath}",
            Global.ProgramExtension, loadedAssembly, targetPath);

        Environment.Exit(0);
    }

    private void TargetDotnetCore(string? assembly, Assembly loadedAssembly, string targetName, string targetPath, Version version)
    {
        if (!_manifestService.CreateCpzManifestXml(
                assembly: loadedAssembly,
                targetName: targetName,
                targetPath: targetPath,
                version,
                out var manifest))
        {
            _logger.LogError("Failed creating the manifest from the assembly {assembly}", assembly);
            Environment.Exit(2);
        }

        manifest!.Save(_manifestService.CreateManifestFileName(targetPath));

        if (!_archiveService.CreateSimplSharpArchive(TargetType.Library, targetPath, version))
        {
            _logger.LogError("Failed creating the archive from the assembly {assembly}", assembly);
            Environment.Exit(2);
        }

        _logger.LogInformation("Successfully created {extension} from {assembly} located at {targetPath}",
            Global.ProgramExtension, loadedAssembly, targetPath);

        Environment.Exit(0);
    }
}