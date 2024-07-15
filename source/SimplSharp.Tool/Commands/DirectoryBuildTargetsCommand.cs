using Cocona;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SimplSharp.Tool;

/// <summary>
/// Creates or modifies a the Directory.Build.targets file in the provided directory.
/// </summary>
/// <param name="logger">a logger with context</param>
/// <param name="buildTargetsService">targets service used to create build targets</param>
internal sealed class DirectoryBuildTargetsCommand(ILogger<DirectoryBuildTargetsCommand> logger, BuildTargetsService buildTargetsService)
{

    [Command("targets", Aliases = new string[] { "t" }, Description = "Creates or modifies a the Directory.Build.targets file in the provided directory.")]
    public void CreateTargets(
        [Option(
            shortName: 'd',
            Description = "Directory to scan for an existing Directory.Build.targets file",
            ValueName = "directory")] string directory)
    {

        var info = new DirectoryInfo(directory);

        if (!info.Exists)
        {
            logger.LogError("Target directory not found {directory}", directory);
            Environment.Exit(1);
            return;
        }

        try
        {
            var result = buildTargetsService.CreateOrModifyDirectoryTargets(info);  

            logger.LogInformation("Completed directory build targets with result {targetResults}", result);

            if (result == BuildTargetsService.BuildTargetsResult.DirectoryNotFound)
            {
                Environment.Exit(1);
                return;
            }

            Environment.Exit(0);
        }

        catch (Exception e)
        {
            logger.LogError(e, "Failed to create directory targets in the provided path {directory}", directory);
            Environment.Exit(100);
        }
    }
}