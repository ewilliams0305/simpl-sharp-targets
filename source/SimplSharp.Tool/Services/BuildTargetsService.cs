using Microsoft.Extensions.Logging;

namespace SimplSharp.Tool;

internal sealed class BuildTargetsService
{
    private readonly ILogger<BuildTargetsService> _logger;
    private readonly ProjectService _projectService;

    public BuildTargetsService(ILogger<BuildTargetsService> logger, ProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }


    public BuildTargetsResult
}

/// <summary>
/// The results of the directory build target functions.
/// </summary>
internal enum BuildTargetsResult
{
    /// <summary>
    /// A new file was created
    /// </summary>
    Created,
    /// <summary>
    /// An existing file was modified.
    /// </summary>
    Modified,
    /// <summary>
    /// The path specified does not exist
    /// </summary>
    DirectoryNotFound,
    /// <summary>
    /// An exception happened while processing the file.
    /// </summary>
    Exception,
}