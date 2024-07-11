#pragma warning disable IDE0290
#pragma warning disable IDE0300

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

    /// <summary>
    /// Creates a new instance of the command
    /// </summary>
    /// <param name="logger">A provided console logger</param>
    /// <param name="manifestService">The manifest service to compile a manifest information file.</param>
    public ClzCommand(ILogger<ClzCommand> logger, ManifestService manifestService, ArchiveService archiveService)
    {
        _logger = logger;
        _manifestService = manifestService;
        _archiveService = archiveService;
    }


    [Command("clz", Aliases = new string []{"l"}, Description = "Creates a CLZ from the target assembly DLL referencing the SimplSharp SDK")]
    public void CreateDriver(
        [Option(
            shortName: 'p',
            Description = "File path for the compiled assembly that will be converted to a SimplSharp CLZ",
            ValueName = "path")] string path,
        [Option(
            shortName: 'd',
            Description = "Newly created CPZ destination",
            ValueName = "destination")] string? destination)
    {

        var info = new FileInfo(path);

        if (!info.Exists)
        {
            _logger.LogError("Target assembly not found {path}", path);
            Environment.Exit(0);
            return;
        }

        try
        {
            var assembly = Assembly.LoadFrom(path);

            if (assembly is null)
            {
                _logger.LogError("Failed to load the assembly at {path}", path);
                Environment.Exit(2);
                return;
            }

            _logger.LogInformation("Creating a SIMPL Sharp Library from the provided assembly {assembly}", assembly);

            var assemblyName = assembly.GetName();
            var targetName = assemblyName.Name + Global.LibraryExtension;
            var targetPath = destination is not null
                ? Path.Combine(destination, targetName)
                : path.Replace(".dll", Global.LibraryExtension);

            if (!_manifestService.CreateClzManifestXml(
                    assembly: assembly, 
                    targetName: targetName, 
                    targetPath: targetPath, 
                    out var manifest))
            {
                _logger.LogError("Failed creating the manifest from the assembly {assembly}", assembly);
                Environment.Exit(2);
                return;
            }

            manifest!.Save(_manifestService.CreateManifestFileName(targetPath));

            if (!_archiveService.CreateSimplSharpArchive(TargetType.Library, targetPath))
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