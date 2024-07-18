#pragma warning disable IDE0290
#pragma warning disable CA1822

using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace SimplSharp.Tool;

internal sealed class ArchiveService
{
    private readonly ILogger<ArchiveService> _logger;

    public ArchiveService(ILogger<ArchiveService> logger)
    {
        _logger = logger;
    }

    public bool CreateSimplSharpArchive(TargetType type, string targetAssemblyFilepath)
    {
        try
        {
            var outputPath = new FileInfo(targetAssemblyFilepath).Directory!.FullName;
            var archiveName = CreateArchiveFilepath(type, targetAssemblyFilepath);

            if (File.Exists(archiveName))
            {
                File.Delete(archiveName);
            }

            using var zipStream = new FileStream(archiveName, FileMode.Create, FileAccess.Write);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);
            
            var outputFiles = Directory.GetFiles(outputPath, "*.*", SearchOption.AllDirectories);
            var copyOnvif = File.Exists(Path.Combine(outputPath, "SimplSharpOnvifInterface.dll"));

            foreach (var filename in outputFiles)
            {
                if (IsFileArchive(filename))
                {
                    continue;
                }

                if (filename.Equals(archiveName, StringComparison.OrdinalIgnoreCase) ||
                    (Path.GetFileName(filename).Equals("CrestronOnvif.dll", StringComparison.OrdinalIgnoreCase) && !copyOnvif))
                    continue;

                var fileInfo = new FileInfo(filename);

                using var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                using var fileStreamInZip = archive.CreateEntry(fileInfo.Name).Open();
                fileStream.CopyTo(fileStreamInZip);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create archive {targetType} {path}", type, targetAssemblyFilepath);
            return false;
        }
    }

    /// <summary>
    /// Creates a target file path for an assembly.
    /// </summary>
    /// <param name="type">The type of SIMPL Sharp project</param>
    /// <param name="assemblyFilePath">The target assembly.</param>
    /// <returns>fully qualified file path.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string CreateArchiveFilepath(TargetType type, string assemblyFilePath) =>
        type switch
        {
            TargetType.Library => assemblyFilePath.Replace(".dll", Global.LibraryExtension),
            TargetType.Program => assemblyFilePath.Replace(".dll", Global.ProgramExtension),
            TargetType.ProgramLibrary => assemblyFilePath.Replace(".dll", Global.ProgramLibraryExtension),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    private static bool IsFileArchive(string filename) =>
        filename.EndsWith(".zip") ||
        filename.EndsWith(Global.LibraryExtension) ||
        filename.EndsWith(Global.ProgramExtension) ||
        filename.EndsWith(Global.ProgramLibraryExtension);
}
