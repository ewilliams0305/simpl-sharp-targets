using System.Diagnostics;

namespace SimplSharp.Tool;

internal sealed class ProcessService
{

    public async Task<(string output, int exitCode)> Execute(string executableName, string? args = null)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = executableName,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = args,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return (output, process.ExitCode);
    }
    
}