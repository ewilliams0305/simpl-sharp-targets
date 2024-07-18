using Cocona;
using Microsoft.Extensions.Logging;

namespace SimplSharp.Tool;

/// <summary>
/// Queries a csproj file for information regarding the SimplSharp SDK
/// </summary>
/// <param name="logger">a logger with context</param>
/// <param name="projectService">targets service used to create build targets</param>
internal sealed class ProjectQueryCommand(ILogger<ProjectQueryCommand> logger, ProjectService projectService)
{

    [Command("query", Aliases = new string[] { "q" }, Description = "Queries a csproj file for information regarding the SimplSharp SDK")]
    public void QueryLibraryVersion(
        [Option(
            shortName: 'p',
            Description = "Full path for a csproj file to query",
            ValueName = "project")] string csprojFilePath)
    {

        if (!File.Exists(csprojFilePath))
        {
            logger.LogError("Target Project was not found {csprojFilePath}", csprojFilePath);
            Environment.Exit(1);
            return;
        }

        try
        {
            var libraryVersion = projectService.QueryProjectForSimplSharpLibraryVersion(csprojFilePath);  
            var programVersion = projectService.QueryProjectForSimplSharpProgramVersion(csprojFilePath);  

            if (libraryVersion is null && programVersion is null)
            {
                logger.LogWarning("The target project does not reference the SIMPL Sharp SDK {project}", csprojFilePath);
                Environment.Exit(3);
                return;
            }

            logger.LogWarning("Target project versions:\nSIMPL Sharp Library: {libVersion}\nSIMPL Sharp Program: {proVersion}", libraryVersion, programVersion);
            Environment.Exit(0);
        }

        catch (Exception e)
        {
            logger.LogWarning(e,"Unhanded exception parsing {project}", csprojFilePath);
            Environment.Exit(100);
        }
    }
}