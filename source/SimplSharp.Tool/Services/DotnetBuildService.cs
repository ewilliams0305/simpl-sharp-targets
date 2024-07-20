namespace SimplSharp.Tool;

public sealed class DotnetBuildService
{
    private readonly ProcessService _execute;

    public DotnetBuildService(ProcessService execute)
    {
        _execute = execute;
    }
    public async Task BuildProject(string targetProjectOrDirectory)
    {
        var result = await _execute.Execute("dotnet", args: $"build {targetProjectOrDirectory}");

        if (result.exitCode != 0)
        {
            throw new Exception($"Failed to compile project {targetProjectOrDirectory}");
        }
    }
}