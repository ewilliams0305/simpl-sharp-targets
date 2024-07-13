using Cocona;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SimplSharp.Tool;

internal sealed class DirectoryBuildTargetsCommand
{
    private readonly ILogger<DirectoryBuildTargetsCommand> _logger;
    private readonly BuildTargetsService _buildTargetsService;
    private readonly ProjectService _projectService;

    public DirectoryBuildTargetsCommand(ILogger<DirectoryBuildTargetsCommand> logger, BuildTargetsService buildTargetsService, ProjectService projectService)
    {
        _logger = logger;
        _buildTargetsService = buildTargetsService;
        _projectService = projectService;
    }


    [Command("targets", Aliases = new string[] { "l" }, Description = "Creates or modifies a the Directory.Build.targets file in the provided directory.")]
    public void CreateDriver(
        [Option(
            shortName: 'd',
            Description = "Directory to scan for an existing Directory.Build.targets file",
            ValueName = "directory")] string directory)
    {

        var info = new DirectoryInfo(directory);

        if (!info.Exists)
        {
            _logger.LogError("Target directory not found {directory}", directory);
            Environment.Exit(1);
            return;
        }

        try
        {
          
            Environment.Exit(0);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create directory targets in the provided path {directory}", directory);
            Environment.Exit(100);
        }
    }
}